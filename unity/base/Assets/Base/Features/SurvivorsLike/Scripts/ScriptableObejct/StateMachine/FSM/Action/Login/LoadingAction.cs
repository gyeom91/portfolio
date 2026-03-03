using UnityEngine;

[CreateAssetMenu(fileName = "LoadingAction", menuName = PATH + "Login/LoadingAction")]
public class LoadingAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uILoginPanel = uIService.Open<UILoginPanel>();
        uILoginPanel.SetActiveLoadingBar(true);
        uILoginPanel.SetActiveLoginButtons(false);

        var poolService = sceneController.GetService<PoolService>();
        await poolService.Initialize(OnUpdate, OnSucceeded, OnFailed);
    }

    private void OnUpdate(long total, long download, float percent)
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uILoginPanel = uIService.Get<UILoginPanel>();
        uILoginPanel.SetLoadingBarWithText(total, download, percent);
    }

    private void OnSucceeded()
    {
        var textAsset = Resources.Load<TextAsset>("TableContainer");
        TableManager.Instance.LoadTable<CharacterTable>(textAsset);
        TableManager.Instance.LoadTable<HeroTable>(textAsset);
        //TableManager.Instance.LoadTable<ExpTable>(textAsset);
        //TableManager.Instance.LoadTable<LevelingTable>(textAsset);

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uILoginPanel = uIService.Get<UILoginPanel>();
        uILoginPanel.SetActiveLoadingBar(false);
        uILoginPanel.SetActiveLoginButtons(true);
    }

    private void OnFailed(string exception)
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIMessagePanel = uIService.Get<UIMessagePanel>();
        var message = "로그인 실패!";
        uIMessagePanel.OnConfirmClick += this.Exit;
        uIMessagePanel.ShowMessage(UIMessagePanel.ETitle.Error, UIMessagePanel.EMessage.Confirm, message);
    }
}
