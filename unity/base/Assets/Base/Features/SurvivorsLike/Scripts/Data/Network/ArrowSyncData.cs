using Unity.Netcode;
using UnityEngine;

public struct ArrowSyncData : INetworkSerializable
{
    public ulong NetworkID;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref NetworkID);
    }
}
