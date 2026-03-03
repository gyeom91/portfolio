using UnityEngine;

[CreateAssetMenu(fileName = "LobbyMainAction", menuName = PATH + "Lobby/LobbyMainAction")]
public class LobbyMainAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance as LobbySceneController;
        var lobbyActor = sceneController.LocalPlayer;
        lobbyActor?.SetEnableCanvas(true);

        var uIService = sceneController.GetService<UIService>();
        var lobby = LobbyManager.Instance.Lobby;
        if (lobby.IsNull())
        {
            uIService.Open<UILobbyPanel>();
        }
        else
        {
            await FSMModule.Trigger(Constants.LOBBY_STAY);
        }
    }
}
