using UnityEngine;

public class UIProgressPanel : UIPanel
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _eulers;

    protected override void OnShow()
    {
        base.OnShow();

        _canvas.sortingOrder = 99;
    }

    private void Update()
    {
        if (IsActive == false)
            return;

        _target.Rotate(_eulers * Time.unscaledDeltaTime);
    }
}
