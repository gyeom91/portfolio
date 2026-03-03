using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkClientAction", menuName = PATH + "Network/NetworkClientAction")]
public class NetworkClientAction : NetworkFSMAction
{
    public override async void OnClientDisconnectCallback(ulong obj)
    {
        base.OnClientDisconnectCallback(obj);

        var networkManager = NetworkManager.Singleton;
        var disconnectReason = networkManager.DisconnectReason;
        if (string.IsNullOrEmpty(disconnectReason))
            return;

        await FSMModule.Trigger(Constants.SCENE_LOAD);
    }

    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        try
        {
            var lobby = LobbyManager.Instance.Lobby;
            var lobbyData = lobby.Data;
            if (lobbyData.TryGetValue(Constants.RELAY_CODE, out var dataObject) == false)
                return;

            if (string.IsNullOrEmpty(dataObject.Value))
                return;

            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(dataObject.Value);
#if UNITY_WEBGL
            var connectionType = "wss";
#else
            var connectionType = "dtls";
#endif
            var networkManager = NetworkManager.Singleton;
            var networkConfig = networkManager.NetworkConfig;
            var utp = (UnityTransport)networkConfig.NetworkTransport;
            var relayServerData = joinAllocation.ToRelayServerData(connectionType);
            utp.SetRelayServerData(relayServerData);

            var authenticationData = Get<AuthenticationData>();
            var networkConnectionPayload = new NetworkConnectionPayload();
            networkConnectionPayload.PlayerID = authenticationData.PlayerID;
            networkConnectionPayload.PlayerName = authenticationData.PlayerName;

            var payload = JsonUtility.ToJson(networkConnectionPayload);
            var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);
            networkConfig.ConnectionData = payloadBytes;
            if (networkManager.StartClient() == false)
                throw new Exception();

            var sceneManager = networkManager.SceneManager;
            sceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
    {

    }
}
