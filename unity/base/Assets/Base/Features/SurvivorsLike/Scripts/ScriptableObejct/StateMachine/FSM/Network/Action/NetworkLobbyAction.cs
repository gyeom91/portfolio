using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NetworkLobbyAction", menuName = PATH + "Network/NetworkLobbyAction")]
public class NetworkLobbyAction : NetworkFSMAction
{
    public override async Awaitable Enter()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsServer == false)
            return;

        var curCcene = SceneManager.GetActiveScene();
        if (curCcene.name == "Lobby")
            return;

        var clientIds = networkManager.ConnectedClientsIds;
        var length = clientIds.Count;
        for (var i = 0; i < length; ++i)
        {
            var clientId = clientIds[i];
            if (networkManager.LocalClientId == clientId)
                continue;

            networkManager.DisconnectClient(clientId, "Shutdown");
        }

        networkManager.Shutdown();

        await FSMModule.Trigger(Constants.SCENE_LOAD);
    }
}
