using UnityEngine;

[CreateAssetMenu(fileName = "BattleMainAction", menuName = PATH + "Battle/BattleMainAction")]
public class BattleMainAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance as BattleSceneController;
        var uIService = sceneController.GetService<UIService>();
        if ((Application.platform == RuntimePlatform.Android) ||
            (Application.platform == RuntimePlatform.IPhonePlayer))
        {
            uIService.Open<UIJoystickPanel>();
            uIService.Open<UIBattlePanel>(false);
        }
        else
        {
            uIService.Open<UIBattlePanel>();
        }

        if (NetworkHostAction.OfflineMode)
            sceneController.SetFreeze(false);
    }
}
