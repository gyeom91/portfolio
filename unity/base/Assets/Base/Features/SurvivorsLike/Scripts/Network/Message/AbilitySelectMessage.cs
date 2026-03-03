using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AbilitySelectMessage : INetworkMessage
{
    public Constants.EHeader EHeader;
    public Constants.EAbility EAbility;
    public int DataKey;
    public ulong NetworkObjectID;

    public void NetworkSerialize<T>(Unity.Netcode.BufferSerializer<T> serializer) where T : Unity.Netcode.IReaderWriter
    {
        serializer.SerializeValue(ref EHeader);
        serializer.SerializeValue(ref EAbility);
        serializer.SerializeValue(ref DataKey);
        serializer.SerializeValue(ref NetworkObjectID);
    }
}
