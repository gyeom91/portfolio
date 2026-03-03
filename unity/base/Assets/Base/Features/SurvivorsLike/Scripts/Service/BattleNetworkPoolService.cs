using Unity.Netcode;
using UnityEngine;

public class BattleNetworkPoolService : NetworkPoolService
{
    protected override INetworkPrefabInstanceHandler CreateSpawnHandler(GameObject prefab)
    {
        switch (prefab.name)
        {
            case "Arrow": return new NetworkSpawnHandler<ArrowSyncData>(NetworkManager.Singleton, prefab);
            case "Exp": return new NetworkSpawnHandler<ExpSyncData>(NetworkManager.Singleton, prefab);
        }

        return base.CreateSpawnHandler(prefab);
    }
}
