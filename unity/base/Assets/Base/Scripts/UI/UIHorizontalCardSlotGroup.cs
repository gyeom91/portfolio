using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHorizontalCardSlotGroup : UI
{
    [SerializeField] protected GameObject _uICardSlotPrefab;
    protected List<UICardSlot> _uICardSlots = new();
    protected UICardSlot _selectedUICardSlot = null;

    public void CreateUICardSlot(int slotCount)
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        for (var i = 0; i < slotCount; ++i)
        {
            poolService.Spawn(_uICardSlotPrefab, transform);
        }
        _uICardSlots.AddRange(GetComponentsInChildren<UICardSlot>(true));

        var length = _uICardSlots.Count;
        for (var i = 0; i < length; ++i)
        {
            var uICardSlot = _uICardSlots[i];
            uICardSlot.CreateUICard(slotCount--);
            uICardSlot.OnStartDrag += OnStartDrag;
            uICardSlot.OnStopDrag += OnStopDrag;
        }
    }

    protected virtual void Update()
    {
        if (_selectedUICardSlot.IsNull())
            return;

        var length = _uICardSlots.Count;
        for (var i = 0; i < length; ++i)
        {
            var uICardSlot = _uICardSlots[i];
            if (_selectedUICardSlot.transform.position.x > uICardSlot.transform.position.x)
            {
                if (_selectedUICardSlot.transform.parent.GetSiblingIndex() < uICardSlot.transform.parent.GetSiblingIndex())
                {
                    Swap(uICardSlot);
                    break;
                }
            }

            if (_selectedUICardSlot.transform.position.x < uICardSlot.transform.position.x)
            {
                if (_selectedUICardSlot.transform.parent.GetSiblingIndex() > uICardSlot.transform.parent.GetSiblingIndex())
                {
                    Swap(uICardSlot);
                    break;
                }
            }
        }
    }

    protected virtual void Swap(UICardSlot uICardSlot)
    {
        Transform focusedParent = _selectedUICardSlot.transform.parent;
        Transform crossedParent = uICardSlot.transform.parent;

        uICardSlot.transform.SetParent(focusedParent);
        uICardSlot.transform.localPosition = Vector3.zero;
        uICardSlot.transform.localRotation = Quaternion.identity;
        uICardSlot.transform.localScale = Vector3.one;
        _selectedUICardSlot.transform.SetParent(crossedParent);
    }

    protected virtual void OnStartDrag(UICardSlot obj)
    {
        _selectedUICardSlot = obj;
    }

    protected virtual void OnStopDrag(UICardSlot obj)
    {
        if (_selectedUICardSlot.IsNull())
            return;

        _selectedUICardSlot.transform.localPosition = Vector3.zero;
        _selectedUICardSlot.transform.localRotation = Quaternion.identity;
        _selectedUICardSlot.transform.localScale = Vector3.one;
        _selectedUICardSlot = null;
    }
}
