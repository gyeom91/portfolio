using Unity.Netcode;
using UnityEngine;

public struct ArrowSyncData : INetworkSerializable
{
    public ulong OwnerID;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref OwnerID);
    }
}
