using Unity.Netcode;
using UnityEngine;

public struct EmptySyncData : INetworkSerializable
{
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

    }
}
