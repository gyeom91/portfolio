using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    private Dictionary<Type, Table> _container = new();

    public T1 GetTable<T1>(T1 type = null) where T1 : Table
    {
        var key = typeof(T1);
        if (_container.TryGetValue(key, out var table) == false)
            return null;

        return table as T1;
    }

    public void LoadTable<T>(TextAsset textAsset) where T : Table
    {
        var key = typeof(T);
        if (_container.ContainsKey(key))
            return;

        _container.Add(key, JsonConvert.DeserializeObject<T>(textAsset.ToString()));
    }
}
