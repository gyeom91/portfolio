using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NetworkHostOnlineAction", menuName = PATH + "Network/NetworkHostOnlineAction")]
public class NetworkHostOnlineAction : NetworkHostAction
{
    public override async void OnClientConnectedCallback(ulong obj)
    {
        var payloadData = Get<NetworkConnectionPayloadData>();
        var lobby = LobbyManager.Instance.Lobby;
        var players = lobby.Players;
        if (players.Count != payloadData.Count)
            return;

        await Awaitable.NextFrameAsync();

        base.OnClientConnectedCallback(obj);

        var lobbyData = lobby.Data;
        var dataObject = lobbyData["Scene"];
        var networkManager = NetworkManager.Singleton;
        var sceneManager = networkManager.SceneManager;
        sceneManager.LoadScene(dataObject.Value, LoadSceneMode.Single);
    }

    public override async Awaitable Enter()
    {
        OfflineMode = false;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        try
        {
            var lobby = LobbyManager.Instance.Lobby;
            var allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
#if UNITY_WEBGL
            var connectionType = "wss";
#else
            var connectionType = "dtls";
#endif
            var networkManager = NetworkManager.Singleton;
            var networkConfig = networkManager.NetworkConfig;
            var utp = (UnityTransport)networkConfig.NetworkTransport;
            var relayServerData = allocation.ToRelayServerData(connectionType);
            utp.SetRelayServerData(relayServerData);

            await base.Enter();

            var authenticationData = Get<AuthenticationData>();
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            var updateLobbyOptions = new UpdateLobbyOptions()
            {
                IsLocked = true,
                Data = new Dictionary<string, DataObject>()
                {
                    { Constants.RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                },
            };
            await LobbyManager.Instance.UpdateLobbyOptions(authenticationData.PlayerID, updateLobbyOptions);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    protected override async void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        await Awaitable.NextFrameAsync();

        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<NetworkPoolService>();
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        var networkManager = NetworkManager.Singleton;
        var spawnManager = networkManager.SpawnManager;
        var networkConnectionPayloadData = Get<NetworkConnectionPayloadData>();
        var clients = networkManager.ConnectedClientsList;
        var length = clients.Count;
        for (var i = 0; i < length; ++i)
        {
            var clientId = clients[i].ClientId;
            var networkConnectionPayload = networkConnectionPayloadData.Get(clientId);
            if (networkConnectionPayload.Equals(default))
                continue;

            var characterName = LobbyManager.Instance.GetPlayerData(networkConnectionPayload.PlayerID, "CharacterName");
            if (string.IsNullOrEmpty(characterName))
                continue;

            var character = await poolService.LoadAsync<GameObject>($"Battle_{characterName}");
            if (character.TryGetComponent<NetworkObject>(out var networkObject) == false)
                continue;

            var randomPos = battleWorldService.GetRandomPosition();
            spawnManager.InstantiateAndSpawn(networkPrefab: networkObject, ownerClientId: clientId, destroyWithScene: true, position: randomPos);
        }

        base.OnLoadEventCompleted(sceneName, loadSceneMode, clientsCompleted, clientsTimedOut);
    }
}
