using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct NetworkConnectedSyncData : INetworkSerializable
{
    public FixedString32Bytes PlayerID;
    public FixedString32Bytes PlayerName;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerID);
        serializer.SerializeValue(ref PlayerName);
    }

    public static implicit operator NetworkConnectedSyncData(NetworkConnectionPayload networkConnectionPayload)
    {
        return new NetworkConnectedSyncData()
        {
            PlayerID = new FixedString32Bytes(networkConnectionPayload.PlayerID),
            PlayerName = new FixedString32Bytes(networkConnectionPayload.PlayerName),
        };
    }
}
