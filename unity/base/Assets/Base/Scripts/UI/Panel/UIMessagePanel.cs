using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessagePanel : UIPanel
{
    public enum EMessage { Confirm, YesNo }
    public enum ETitle { Error, Warring, }

    public event Action OnConfirmClick;
    public event Action OnYesClick;
    public event Action OnNoClick;

    [SerializeField] private GameObject _confirmGroup;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private GameObject _yesNoGroup;
    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;
    [SerializeField] private TextMeshProUGUI _titleTMPro;
    [SerializeField] private TextMeshProUGUI _messageTMPro;

    public void ShowMessage(ETitle eTitle, EMessage eMessage, string message)
    {
        _titleTMPro.text = eTitle.ToString();
        _messageTMPro.text = message;

        switch (eMessage)
        {
            case EMessage.Confirm:
                {
                    _confirmGroup.SetActive(true);
                    _yesNoGroup.SetActive(false);

                    break;
                }

            case EMessage.YesNo:
                {
                    _confirmGroup.SetActive(false);
                    _yesNoGroup.SetActive(true);

                    break;
                }
        }

        Show();
    }

    protected override void OnShow()
    {
        base.OnShow();

        _canvas.sortingOrder = 98;
    }

    protected override void OnHide()
    {
        base.OnHide();

        OnConfirmClick = null;
        OnYesClick = null;
        OnNoClick = null;
    }

    protected override void Awake()
    {
        base.Awake();

        _confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        _yesBtn.onClick.AddListener(OnYesBtnClick);
        _noBtn.onClick.AddListener(OnNoBtnClick);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _confirmBtn.onClick.RemoveListener(OnConfirmBtnClick);
        _yesBtn.onClick.RemoveListener(OnYesBtnClick);
        _noBtn.onClick.RemoveListener(OnNoBtnClick);

        OnConfirmClick = null;
        OnYesClick = null;
        OnNoClick = null;
    }

    private void OnConfirmBtnClick()
    {
        OnConfirmClick?.Invoke();

        OnClickedCloseBtn();
    }

    private void OnYesBtnClick()
    {
        OnYesClick?.Invoke();

        OnClickedCloseBtn();
    }

    private void OnNoBtnClick()
    {
        OnNoClick?.Invoke();

        OnClickedCloseBtn();
    }
}
