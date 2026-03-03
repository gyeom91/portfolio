using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ExpIncreaseMessage : INetworkMessage
{
    public Constants.EHeader EHeader;
    public int MaxValue;
    public int Value;

    public void NetworkSerialize<T>(Unity.Netcode.BufferSerializer<T> serializer) where T : Unity.Netcode.IReaderWriter
    {
        serializer.SerializeValue(ref EHeader);
        serializer.SerializeValue(ref MaxValue);
        serializer.SerializeValue(ref Value);
    }
}
