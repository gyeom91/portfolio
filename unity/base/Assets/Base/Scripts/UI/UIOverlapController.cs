using System.Collections.Generic;
using UnityEngine;

public class UIOverlapController : UI
{
    [SerializeField] private UIOverlapElement _uIOverlapElementPrefab;
    [SerializeField] private UIOverlapElementGroup _uIOverlapElementGroupPrefab;
    private Camera _mainCamera = null;
    private List<UIOverlapElement> _uIOverlapElements = new();
    private List<UIOverlapElementGroup> _uIOverlapElementGroups = new();

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
    }

    private void UpdateRoutine()
    {
        UpdateDeactiveGroup();
        UpdateElements();
        CalculateOverlapGroupID();
        UpdateDeactiveGroupElement();
        CreateGroup();
        UpdateGroups();
    }

    private void UpdateDeactiveGroup()
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        var length = _uIOverlapElementGroups.Count;
        for (var i = 0; i < length; ++i)
        {
            var uIOverlapElementGroup = _uIOverlapElementGroups[i];
            uIOverlapElementGroup.SetGroupID(-1);
            uIOverlapElementGroup.ClearElements();

            poolService.Despawn(uIOverlapElementGroup);
        }

        _uIOverlapElementGroups.Clear();
    }

    private void UpdateElements()
    {
        var length = _uIOverlapElements.Count;
        for (var i = 0; i < length; ++i)
        {
            var uIOverlapElement = _uIOverlapElements[i];
            uIOverlapElement.SetGroupID(-1);
            uIOverlapElement.UpdatePosition(_mainCamera);
        }
    }

    private void CalculateOverlapGroupID()
    {
        for (var i = 0; i < _uIOverlapElements.Count; ++i)
        {
            var left = _uIOverlapElements[i];
            if (left.IsActive == false)
                continue;

            if (left.GroupID != -1)
                continue;

            left.SetGroupID(i);

            for (var j = 0; j < _uIOverlapElements.Count; ++j)
            {
                var right = _uIOverlapElements[j];
                if (right == left)
                    continue;

                if (right.IsActive == false)
                    continue;

                var rectTransform = left.rectTransform;
                if (rectTransform.IsOverlapMarker(right.rectTransform) == false)
                    continue;

                right.SetGroupID(i);
            }
        }
    }

    private void UpdateDeactiveGroupElement()
    {
        var length = _uIOverlapElements.Count;
        for (var i = 0; i < length; ++i)
        {
            var left = _uIOverlapElements[i];
            var hasGroup = false;
            for (var j = 0; j < length; ++j)
            {
                var right = _uIOverlapElements[j];
                if (left == right)
                    continue;

                if (left.GroupID == right.GroupID)
                {
                    hasGroup = true;
                    break;
                }
            }

            if (hasGroup)
            {
                left.SetActive(false);
            }
            else
            {
                left.SetGroupID(-1);
            }
        }
    }

    private void CreateGroup()
    {
        var sceneController = SceneController.Instance;
        var length = _uIOverlapElements.Count;
        for (var i = 0; i < length; ++i)
        {
            var uIOverlapElement = _uIOverlapElements[i];
            if (uIOverlapElement.GroupID == -1)
                continue;

            var poolService = sceneController.GetService<PoolService>();
            var uIOverlapElementGroup = GetUIOverlapElementGroupWithGroupID(uIOverlapElement.GroupID);
            if (uIOverlapElementGroup.IsNull())
            {
                uIOverlapElementGroup = poolService.Spawn(_uIOverlapElementGroupPrefab, transform);
                uIOverlapElementGroup.SetGroupID(uIOverlapElement.GroupID);

                _uIOverlapElementGroups.Add(uIOverlapElementGroup);
            }

            uIOverlapElementGroup.AddElement(uIOverlapElement);
        }
    }

    private void UpdateGroups()
    {
        var length = _uIOverlapElementGroups.Count;
        for (var i = 0; i < length; ++i)
            _uIOverlapElementGroups[i].UpdatePosition(_mainCamera);
    }

    private UIOverlapElementGroup GetUIOverlapElementGroupWithGroupID(int groupID)
    {
        var length = _uIOverlapElementGroups.Count;
        for (var i = 0; i < length; ++i)
        {
            var uIOverlapElementGroup = _uIOverlapElementGroups[i];
            if (uIOverlapElementGroup.GroupID == groupID)
                return uIOverlapElementGroup;
        }

        return null;
    }
}
