using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class NetworkManagerExtension
{
    public static NetworkObject GetNetworkObject(this NetworkManager networkManager, ulong id)
    {
        var spawnManager = networkManager.SpawnManager;
        if (spawnManager.IsNull())
            return null;

        var spawnedObjects = spawnManager.SpawnedObjects;
        if (spawnedObjects.IsNull())
            return null;

        return spawnedObjects.GetValueOrDefault(id);
    }
}
