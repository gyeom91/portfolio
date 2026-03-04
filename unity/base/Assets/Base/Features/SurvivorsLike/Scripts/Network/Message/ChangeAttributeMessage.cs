using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ChangeAttributeMessage : INetworkMessage
{
    public Constants.EHeader EHeader;
    public FixedString128Bytes AttributeName;
    public float BaseValue;
    public float CurrentValue;
    public ulong NetworkObjectID;

    public void NetworkSerialize<T>(Unity.Netcode.BufferSerializer<T> serializer) where T : Unity.Netcode.IReaderWriter
    {
        serializer.SerializeValue(ref EHeader);
        serializer.SerializeValue(ref NetworkObjectID);
        serializer.SerializeValue(ref AttributeName);
        serializer.SerializeValue(ref BaseValue);
        serializer.SerializeValue(ref CurrentValue);
    }
}
