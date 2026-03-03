using Unity.Services.Authentication;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNameUpdateAction", menuName = PATH + "Login/PlayerNameUpdateAction")]
public class PlayerNameUpdateAction : FSMAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIPlayerNameUpdatePanel = uIService.Open<UIPlayerNameUpdatePanel>();
        uIPlayerNameUpdatePanel.GetConfirmClickedEvent().AddListener(OnClickedConfirmBtn);
    }

    public override async Awaitable Exit()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIPlayerNameUpdatePanel = uIService.Get<UIPlayerNameUpdatePanel>();
        uIPlayerNameUpdatePanel.GetConfirmClickedEvent().RemoveListener(OnClickedConfirmBtn);
    }

    private void OnClickedConfirmBtn()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIMessagePanel = uIService.Get<UIMessagePanel>();
        var message = "저장확인!";
        uIMessagePanel.OnYesClick += OnYesClick;
        uIMessagePanel.OnNoClick += OnNoClick;
        uIMessagePanel.ShowMessage(UIMessagePanel.ETitle.Error, UIMessagePanel.EMessage.YesNo, message);
    }

    private async void OnYesClick()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIPlayerNameUpdatePanel = uIService.Get<UIPlayerNameUpdatePanel>();
        var playerName = uIPlayerNameUpdatePanel.GetPlayerNameText();
        uIService.Open<UIProgressPanel>(false);

        var authenticationService = AuthenticationService.Instance;
        var updatePlayerName = await authenticationService.UpdatePlayerNameAsync(playerName);
        var authenticationData = Get<AuthenticationData>();
        authenticationData.SetPlayerID(authenticationService.PlayerId);
        authenticationData.SetPlayerName(updatePlayerName);

        await FSMModule.Trigger(Constants.SCENE_LOAD);
    }

    private void OnNoClick()
    {

    }
}
