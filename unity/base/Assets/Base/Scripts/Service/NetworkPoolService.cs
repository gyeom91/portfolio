using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPoolService : PoolService
{
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;
    private List<INetworkPrefabInstanceHandler> _handlers = new();

    public NetworkObject HandlerSpawn<T>(string prefabName, T instanceData, Vector3 position = default, Quaternion rotation = default) where T : struct, INetworkSerializable
    {
        var length = _handlers.Count;
        for (var i = 0; i < length; ++i)
        {
            if (_handlers[i] is NetworkSpawnHandler<T> spawnHandler == false)
                continue;

            var prefab = spawnHandler.Prefab;
            if (prefab.name != prefabName)
                continue;

            var pos = position == Vector3.zero ? Vector3.zero : position;
            var rot = rotation == Quaternion.identity ? Quaternion.identity : rotation;
            return spawnHandler.Instantiate(pos, rot, instanceData);
        }

        return null;
    }

    public GameObject ServerSpawn(GameObject prefab, Transform parent = null, int defaultSpawnCount = 10)
    {
        var clone = Spawn(prefab, parent, defaultSpawnCount);
        if (clone.TryGetComponent<NetworkObject>(out var networkObject) == false)
            return clone;

        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsNull() || networkManager.IsServer == false)
            return clone;

        networkObject.Spawn();
        return clone;
    }

    protected override void Awake()
    {
        base.Awake();

        var networkPrefabs = _networkPrefabsList.PrefabList;
        var length = networkPrefabs.Count;
        for (var i = 0; i < length; ++i)
        {
            var prefab = networkPrefabs[i].Prefab;
            var handler = CreateSpawnHandler(prefab);
            if (handler.IsNull())
                continue;

            _handlers.Add(handler);
        }
    }

    protected virtual INetworkPrefabInstanceHandler CreateSpawnHandler(GameObject prefab)
    {
        if (prefab.name == "NetworkPlayer")
            return null;

        return new NetworkSpawnHandler<EmptySyncData>(NetworkManager.Singleton, prefab);
    }
}
