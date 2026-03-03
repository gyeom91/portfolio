using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPanel : UIPanel
{
    [SerializeField] private GameObject _progressGroup;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private GameObject _loginBtnGroup;
    [SerializeField] private Button _btnSignIn;
    [SerializeField] private Button _btnGuest;

    public void SetActiveLoadingBar(bool active)
    {
        _progressGroup.SetActive(active);
        _progressText.gameObject.SetActive(active);
    }

    public void SetActiveLoginButtons(bool active)
    {
        _loginBtnGroup.SetActive(active);
    }

    public void SetLoadingBarWithText(long totalBytes, long download, float percent)
    {
        _progressSlider.value = percent;
        _progressText.text = $"{string.Format("{0:F2}", percent * 100)}%";
    }

    protected override void OnShow()
    {
        base.OnShow();

        _progressText.text = $"{0}%";
        _progressSlider.value = 0;
        _progressSlider.maxValue = 1;
    }

    protected override void Awake()
    {
        base.Awake();

        _btnSignIn.onClick.AddListener(OnClickedSignInBtn);
        _btnGuest.onClick.AddListener(OnClickedGuestInBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _btnSignIn.onClick.RemoveListener(OnClickedSignInBtn);
        _btnGuest.onClick.RemoveListener(OnClickedGuestInBtn);
    }

    private async void OnClickedSignInBtn()
    {
        await FSMModule.Trigger(Constants.LOGIN_SOCIAL_SIGN_IN);
    }

    private async void OnClickedGuestInBtn()
    {
        await FSMModule.Trigger(Constants.LOGIN_GUEST_SIGN_IN);
    }
}
