using System;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbySearchAction", menuName = PATH + "Lobby/LobbySearchAction")]
public class LobbySearchAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance as LobbySceneController;
        var lobbyActor = sceneController.LocalPlayer;
        lobbyActor?.SetEnableCanvas(false);

        var uIService = sceneController.GetService<UIService>();
        var uILobbySearchPanel = uIService.Open<UILobbySearchPanel>();
        uIService.Open<UIProgressPanel>(false);

        try
        {
            var authenticationData = Get<AuthenticationData>();
            await LobbyManager.Instance.LeaveOrDelete(authenticationData.PlayerID);

            var options = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>() { new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) },
                Order = new List<QueryOrder>() { new QueryOrder(false, QueryOrder.FieldOptions.Created) },
            };
            var response = await LobbyService.Instance.QueryLobbiesAsync(options);
            var lobbies = response.Results;
            uILobbySearchPanel.Refresh(lobbies);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            uIService.Close<UIProgressPanel>();
        }
    }

    public override Awaitable Trigger()
    {
        return Enter();
    }
}
