using UnityEngine;
using UnityEngine.UI;

public class UIBattleOptionPanel : UIPanel
{
    [SerializeField] private Button _shutdownBtn;
    [SerializeField] private Button _closeBtn;

    protected override void Awake()
    {
        base.Awake();

        _closeBtn.onClick.AddListener(OnClickedCloseBtn);
        _shutdownBtn.onClick.AddListener(OnClickedShutdownBtn);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == false)
            return;

        await FSMModule.Trigger(Constants.BATTLE_MAIN);
    }

    protected override async void OnClickedCloseBtn()
    {
        base.OnClickedCloseBtn();

        await FSMModule.Trigger(Constants.BATTLE_MAIN);
    }

    private async void OnClickedShutdownBtn()
    {
        await FSMModule.Trigger(Constants.NETWORK_LOBBY);
    }
}
