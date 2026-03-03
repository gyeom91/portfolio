using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.RemoteConfig;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbyStayAction", menuName = PATH + "Lobby/LobbyStayAction")]
public class LobbyStayAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UILobbyStayPanel>();

        var authenticationData = Get<AuthenticationData>();
        await LobbyManager.Instance.UpdatePlayerData(authenticationData.PlayerID, "Ready", $"{false}");

        var updateLobbyOptions = new UpdateLobbyOptions()
        {
            IsLocked = false,
            Data = new Dictionary<string, DataObject>()
                {
                    { Constants.RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, string.Empty) }
                },
        };
        await LobbyManager.Instance.UpdateLobbyOptions(authenticationData.PlayerID, updateLobbyOptions);
    }

    public override async Awaitable Stay()
    {
        var authenticationData = Get<AuthenticationData>();
        var heartbeatTime = RemoteConfigService.Instance.appConfig.GetFloat("HeartbeatTime");
        await LobbyManager.Instance.Heartbeat(authenticationData.PlayerID, heartbeatTime);
    }
}
