using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct LevelUpEnterMessage : INetworkMessage
{
    public Constants.EHeader EHeader;
    public int MaxExpValue;
    public int ExpValue;

    public void NetworkSerialize<T>(Unity.Netcode.BufferSerializer<T> serializer) where T : Unity.Netcode.IReaderWriter
    {
        serializer.SerializeValue(ref EHeader);
        serializer.SerializeValue(ref MaxExpValue);
        serializer.SerializeValue(ref ExpValue);
    }
}
