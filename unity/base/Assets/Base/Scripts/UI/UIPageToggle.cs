using UnityEngine;
using UnityEngine.UI;

public class UIPageToggle : UI
{
    [SerializeField] private UIPage _uIPage;
    private Toggle _toggle = null;

    protected override void Awake()
    {
        base.Awake();

        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChangedValueToggle);
    }

    private void OnChangedValueToggle(bool isOn)
    {
        if (_uIPage.IsNull())
            return;

        if (isOn)
            _uIPage.Show();
        else
            _uIPage.Hide();
    }
}
