using System;
using UnityEngine;

public class UI : BaseMonobehaviour
{
    public override bool IsActive => gameObject.activeSelf;
    public RectTransform rectTransform { get; private set; }
    public event Action<bool> OnActivate;

    public void Show()
    {
        if (IsActive)
            return;

        OnShow();
        OnActivate?.Invoke(true);
    }

    public void Hide()
    {
        if (IsActive == false)
            return;

        OnHide();
        OnActivate?.Invoke(false);
    }

    protected virtual void OnShow()
    {
        SetActive(true);
    }

    protected virtual void OnHide()
    {
        SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();

        rectTransform = transform as RectTransform;
    }
}
