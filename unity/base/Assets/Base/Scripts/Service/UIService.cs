using System;
using System.Collections.Generic;
using UnityEngine;

public class UIService : Service
{
    [SerializeField] private UIPanel[] _uIPanelPrefabs;
    private UIPanel[] _uIPanels = null;
    private Stack<UIPanel> _stack = new();
    private int _openCount = 0;

    public T Get<T>() where T : UIPanel
    {
        return Array.Find(_uIPanels, uIPanel => uIPanel is T) as T;
    }

    public T Open<T>(bool hideOtherPanels = true) where T : UIPanel
    {
        T result = null;
        var length = _uIPanels.Length;
        for (var i = 0; i < length; ++i)
        {
            var uIPanel = _uIPanels[i];
            if (uIPanel is T)
            {
                ++_openCount;
                result = uIPanel as T;
                result.SetSortingOrder(_openCount);
                result.Show();
            }
            else
            {
                if (hideOtherPanels == false)
                    continue;

                uIPanel.Hide();
            }
        }

        return result;
    }

    public void Close<T>() where T : UIPanel
    {
        var length = _uIPanels.Length;
        for (var i = 0; i < length; ++i)
        {
            var uIPanel = _uIPanels[i];
            if (uIPanel is T == false)
                continue;

            --_openCount;
            uIPanel.SetSortingOrder(0);
            uIPanel.Hide();
        }
    }

    public T Next<T>() where T : UIPanel
    {
        if (_stack.TryPop(out var pop))
            pop.Hide();

        var result = Get<T>();
        result.Show();

        _stack.Push(result);

        return result;
    }

    public UIPanel Prev()
    {
        if (_stack.TryPop(out var pop))
            pop.Hide();

        if (_stack.TryPeek(out var peek))
            peek.Show();

        return peek;
    }

    protected override void Awake()
    {
        base.Awake();

        var length = _uIPanelPrefabs.Length;
        for (var i = 0; i < length; ++i)
            Instantiate(_uIPanelPrefabs[i], transform);

        _uIPanels = GetComponentsInChildren<UIPanel>();
    }

    protected virtual void Update()
    {
        var length = _uIPanels.Length;
        for (var i = 0; i < length; ++i)
            _uIPanels[i].UpdateSafeArea();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _uIPanels = null;
    }
}
