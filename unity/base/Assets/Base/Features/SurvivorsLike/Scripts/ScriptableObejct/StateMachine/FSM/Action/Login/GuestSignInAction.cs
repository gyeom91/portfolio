using Unity.Services.Authentication;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestSignInAction", menuName = PATH + "Login/GuestSignInAction")]
public class GuestSignInAction : SignInAction
{
    public override async Awaitable Enter()
    {
        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        await base.Enter();

        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFailed;
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
