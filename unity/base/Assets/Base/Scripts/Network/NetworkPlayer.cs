using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : BaseNetworkBehaviour
{
    private NetworkVariable<NetworkConnectedSyncData> _networkConnectedSyncData = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer == false)
            return;

        var networkConnectionPayloadData = Node.Get<NetworkConnectionPayloadData>();
        var networkConnectionPayload = networkConnectionPayloadData.Get(OwnerClientId);
        if (networkConnectionPayload.Equals(default))
            return;

        _networkConnectedSyncData.Value = networkConnectionPayload;
    }
}
