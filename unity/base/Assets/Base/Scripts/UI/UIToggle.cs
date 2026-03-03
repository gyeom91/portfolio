using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UI
{
    public bool IsOn
    {
        get
        {
            return _toggle.isOn;
        }
        set
        {
            _toggle.isOn = value;
        }
    }

    private Toggle _toggle = null;

    protected override void Awake()
    {
        base.Awake();

        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChangedValueToggle);
    }

    protected virtual void OnChangedValueToggle(bool isOn)
    {

    }
}
