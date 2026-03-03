using UnityEngine;

public class UIWorldToScreen : UI
{
    private Camera _mainCamera = null;
    private Transform _targetTransform = null;

    public void SetTargetTransform(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_mainCamera.IsNull() || _targetTransform.IsNull())
            return;

        var screenPos = _mainCamera.WorldToScreenPoint(_targetTransform.position);
        screenPos.z = 0;

        rectTransform.position = screenPos;
    }
}
