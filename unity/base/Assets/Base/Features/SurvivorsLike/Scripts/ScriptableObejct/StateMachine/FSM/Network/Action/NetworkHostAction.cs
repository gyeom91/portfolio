using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class NetworkHostAction : NetworkFSMAction
{
    public static bool OfflineMode { get; protected set; }

    public override void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        base.OnConnectionApprovalCallback(request, response);

        var connectionData = request.Payload;
        var clientId = request.ClientNetworkId;
        var payload = System.Text.Encoding.UTF8.GetString(connectionData);
        var connectionPayload = JsonUtility.FromJson<NetworkConnectionPayload>(payload);

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;

        var networkConnectionPayloadData = Get<NetworkConnectionPayloadData>();
        networkConnectionPayloadData.Add(clientId, connectionPayload);
    }

    public override void OnClientConnectedCallback(ulong obj)
    {
        base.OnClientConnectedCallback(obj);

        var networkManager = NetworkManager.Singleton;
        var sceneManager = networkManager.SceneManager;
        sceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
    }

    public override async Awaitable Enter()
    {
        var networkManager = NetworkManager.Singleton;
        var authenticationData = Get<AuthenticationData>();
        var networkConnectionPayload = new NetworkConnectionPayload();
        networkConnectionPayload.PlayerID = authenticationData.PlayerID;
        networkConnectionPayload.PlayerName = authenticationData.PlayerName;

        var payload = JsonUtility.ToJson(networkConnectionPayload);
        var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);
        var networkConfig = networkManager.NetworkConfig;
        networkConfig.ConnectionData = payloadBytes;
        if (networkManager.StartHost() == false)
        {
            throw new Exception();
        }
    }

    protected virtual async void OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        await Awaitable.NextFrameAsync();

        var sceneController = SceneController.Instance;
        var networkService = sceneController.GetService<NetworkService>();
        var message = new DummyMessage();
        message.EHeader = Constants.EHeader.LOAD_COMPLETED;

        networkService.SendToAllClient(message, NetworkDelivery.ReliableSequenced);
    }
}
