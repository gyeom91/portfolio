using System;
using UnityEngine;
using Unity.Services.Multiplayer;

[CreateAssetMenu(fileName = "SearchSessionsAction", menuName = PATH + "Session/SearchSessionsAction")]
public class SearchSessionsAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        //var uISearchLobbiesPanel = uIService.OpenPanel<UISearchLobbiesPanel>();
        uIService.Open<UIProgressPanel>(false);

        try
        {
            var response = await MultiplayerService.Instance.QuerySessionsAsync(null);
            var sessionInfos = response.Sessions;
            if (sessionInfos.IsNull() || sessionInfos.Count == 0)
                return;

            //uISearchLobbiesPanel.Refresh(sessionInfos);
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
}
