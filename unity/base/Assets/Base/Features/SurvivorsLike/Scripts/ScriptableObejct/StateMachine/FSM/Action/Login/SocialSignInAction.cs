using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;

[CreateAssetMenu(fileName = "SocialSignInAction", menuName = PATH + "Login/SocialSignInAction")]
public class SocialSignInAction : SignInAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        await base.Enter();

        PlayerAccountService.Instance.SignedIn += OnPlayerAccountSignedIn;
        PlayerAccountService.Instance.SignInFailed += OnSignInFailed;
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    private async void OnPlayerAccountSignedIn()
    {
        var accessToken = PlayerAccountService.Instance.AccessToken;

        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFailed;
        await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
    }
}
