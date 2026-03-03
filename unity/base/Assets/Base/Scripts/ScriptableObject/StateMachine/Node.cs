using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : Asset
{
    private static Dictionary<string, object> _dataDic = new();

    public static T Get<T>(params object[] args) where T : class
    {
        var key = typeof(T).Name;
        if (_dataDic.TryGetValue(key, out var value))
            return value as T;

        value = Create<T>(args);
        _dataDic.Add(key, value);
        return value as T;
    }

    public abstract Awaitable Execute();

    private static T Create<T>(params object[] args) where T : class
    {
        return Activator.CreateInstance(typeof(T), args) as T;
    }
}
