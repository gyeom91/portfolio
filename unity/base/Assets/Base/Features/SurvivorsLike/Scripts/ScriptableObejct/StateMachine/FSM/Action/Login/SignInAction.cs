using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public abstract class SignInAction : FSMAction
{
    public struct userAttributes
    {

    }

    public struct appAttributes
    {

    }

    public override async Awaitable Enter()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
            return;

        await UnityServices.InitializeAsync();
    }

    protected virtual async void OnSignedIn()
    {
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());

        var cloudCode = Get<EconomyModuleBindings>(CloudCodeService.Instance);
        var economies = await cloudCode.GetEconomies();
        var userData = Get<UserData>();
        userData.SetEconomies(economies);

        var authenticationService = AuthenticationService.Instance;
        if (string.IsNullOrEmpty(authenticationService.PlayerName))
        {
            await FSMModule.Trigger(Constants.LOGIN_UPDATE_PLAYER_NAME);
        }
        else
        {
            var authenticationData = Get<AuthenticationData>();
            authenticationData.SetPlayerID(authenticationService.PlayerId);
            authenticationData.SetPlayerName(authenticationService.PlayerName);

            await FSMModule.Trigger(Constants.SCENE_LOAD);
        }
    }

    protected virtual void OnSignInFailed(RequestFailedException exception)
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIMessagePanel = uIService.Get<UIMessagePanel>();
        var message = "입장실패!";
        uIMessagePanel.ShowMessage(UIMessagePanel.ETitle.Error, UIMessagePanel.EMessage.Confirm, message);
    }
}
