using Unity.Netcode;
using UnityEngine;

public class NetworkSpawnHandler<T> : NetworkPrefabInstanceHandlerWithData<T> where T : struct, INetworkSerializable
{
    public NetworkManager NetworkManager { get; private set; }
    public GameObject Prefab { get; private set; }

    public NetworkSpawnHandler(NetworkManager networkManager, GameObject prefab)
    {
        NetworkManager = networkManager;
        Prefab = prefab;

        var prefabHandler = NetworkManager.PrefabHandler;
        prefabHandler.AddHandler(Prefab, this);
    }

    public NetworkObject Instantiate(Vector3 position, Quaternion rotation, T instanceData)
    {
        var clone = SpawnAndSetData(position, rotation, instanceData);
        if (clone.IsNull())
            return null;

        if (NetworkManager.IsNull())
            return null;

        var prefabHandler = NetworkManager.PrefabHandler;
        if (prefabHandler.IsNull())
            return null;

        prefabHandler.SetInstantiationData(clone, instanceData);

        clone.Spawn();
        return clone;
    }

    public override NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation, T instanceData)
    {
        return SpawnAndSetData(position, rotation, instanceData);
    }

    public override void Destroy(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        if (sceneController.IsNull())
            return;

        var poolService = sceneController.GetService<PoolService>();
        if (poolService.IsNull())
            return;

        poolService.Despawn(networkObject);
    }

    protected NetworkObject SpawnAndSetData(Vector3 position, Quaternion rotation, T instanceData)
    {
        var sceneController = SceneController.Instance;
        if (sceneController.IsNull())
            return null;

        var poolService = sceneController.GetService<PoolService>();
        if (poolService.IsNull())
            return null;

        var clone = poolService.Spawn<NetworkObject>(Prefab, position, rotation);
        if (clone.IsNull())
            return null;

        if (clone.TryGetComponents<IInstanceData>(out var instaceDatas) == false)
            return clone;

        var length = instaceDatas.Length;
        for (var i = 0; i < length; ++i)
            instaceDatas[i].SetInstantiationData(NetworkManager, instanceData);

        return clone;
    }
}
