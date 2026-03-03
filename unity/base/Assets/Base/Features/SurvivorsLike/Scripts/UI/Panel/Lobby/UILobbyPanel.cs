using UnityEngine;
using UnityEngine.UI;

public class UILobbyPanel : UIPanel
{
    [SerializeField] private Toggle _mainToggle;
    [SerializeField] private Button _singleplayBtn;
    [SerializeField] private Button _multiplayBtn;

    protected override void Awake()
    {
        base.Awake();

        _singleplayBtn.onClick.AddListener(OnClickedSingleplayBtn);
        _multiplayBtn.onClick.AddListener(OnClickedMultiplayBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _singleplayBtn.onClick.RemoveListener(OnClickedSingleplayBtn);
        _multiplayBtn.onClick.RemoveListener(OnClickedMultiplayBtn);
    }

    protected override void OnShow()
    {
        base.OnShow();

        _mainToggle.isOn = true;
    }

    private async void OnClickedSingleplayBtn()
    {
        await FSMModule.Trigger(Constants.NETWORK_HOST_OFFLINE);
    }

    private async void OnClickedMultiplayBtn()
    {
        await FSMModule.Trigger(Constants.LOBBY_SEARCH);
    }
}
