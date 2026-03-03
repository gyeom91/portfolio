using UnityEngine;

public class UIOverlapElement : UI
{
    public int GroupID { get { return _groupID; } }

    private int _groupID = 0;
    private Transform _targetTransform = null;

    public void SetGroupID(int groupID)
    {
        _groupID = groupID;
    }

    public virtual void UpdatePosition(Camera camera)
    {
        if (_targetTransform.IsNull())
            return;

        var camTransform = camera.transform;
        var screenPos = camera.WorldToScreenPoint(_targetTransform.position);
        var isForward = screenPos.z > 0;
        screenPos.z = 0;
        rectTransform.position = screenPos;

        SetActive(isForward);
    }
}
