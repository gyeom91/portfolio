using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerNameUpdatePanel : UIPanel
{
    [SerializeField] private TMP_InputField _playerNameFld;
    [SerializeField] private Button _confirmBtn;

    public Button.ButtonClickedEvent GetConfirmClickedEvent()
    {
        return _confirmBtn.onClick;
    }

    public string GetPlayerNameText()
    {
        return _playerNameFld.text;
    }

    protected override void OnShow()
    {
        base.OnShow();

        _playerNameFld.text = string.Empty;
    }
}
