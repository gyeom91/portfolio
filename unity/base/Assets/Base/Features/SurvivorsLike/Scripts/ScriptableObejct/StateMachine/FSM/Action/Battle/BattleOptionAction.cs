using UnityEngine;

[CreateAssetMenu(fileName = "BattleOptionAction", menuName = PATH + "Battle/BattleOptionAction")]
public class BattleOptionAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance as BattleSceneController;
        if (NetworkHostAction.OfflineMode)
            sceneController.SetFreeze(true);

        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIBattleOptionPanel>(NetworkHostAction.OfflineMode);
    }
}
