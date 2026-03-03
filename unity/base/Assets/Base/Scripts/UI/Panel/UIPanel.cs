using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
public class UIPanel : UI
{
    public override bool IsActive => base.IsActive && _canvas.enabled;

    protected UIService _uIService { get; private set; }
    protected Canvas _canvas { get; private set; }
    protected GraphicRaycaster _graphicRaycaster { get; private set; }

    [SerializeField] private bool _useSafeArea = true;

    public void SetSortingOrder(int sortingOrder)
    {
        _canvas.sortingOrder = sortingOrder;
    }

    public void UpdateSafeArea()
    {
        if (IsActive == false)
            return;

        if (_useSafeArea == false)
            return;

        rectTransform.SafeArea();
    }

    protected virtual void OnClickedCloseBtn()
    {
        Hide();
    }

    protected override void OnShow()
    {
        _canvas.enabled = true;
        _graphicRaycaster.enabled = true;
    }

    protected override void OnHide()
    {
        _canvas.enabled = false;
        _graphicRaycaster.enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();

        SetActive(true);

        _uIService = GetComponentInParent<UIService>();

        _canvas = GetComponent<Canvas>();
        _canvas.overrideSorting = true;
        _canvas.enabled = false;

        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _graphicRaycaster.enabled = false;
    }

    protected virtual void OnDestroy()
    {
        _uIService = null;
        _canvas = null;
        _graphicRaycaster = null;
    }
}
