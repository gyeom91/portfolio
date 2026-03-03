using System.Collections.Generic;
using UnityEngine;

public class UIOverlapElementGroup : UIOverlapElement
{
    private List<UIOverlapElement> _uIOverlapElements = new();

    public void AddElement(UIOverlapElement uIOverlapElement)
    {
        _uIOverlapElements.Add(uIOverlapElement);
    }

    public void ClearElements()
    {
        _uIOverlapElements.Clear();
    }

    public override void UpdatePosition(Camera camera)
    {
        var totalPosition = Vector3.zero;
        var length = _uIOverlapElements.Count;
        for (var i = 0; i < length; ++i)
        {
            var uIOverlapElement = _uIOverlapElements[i];
            var rectTransform = uIOverlapElement.rectTransform;
            totalPosition += rectTransform.position;
        }

        rectTransform.position = totalPosition / _uIOverlapElements.Count;
    }
}
