using Unity.Netcode;
using UnityEngine;

public struct ExpSyncData : INetworkSerializable
{
    public int DataKey;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref DataKey);
    }
}
