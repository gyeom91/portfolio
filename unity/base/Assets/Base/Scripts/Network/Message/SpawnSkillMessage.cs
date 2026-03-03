using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SpawnSkillMessage : INetworkMessage
{
    public Constants.EHeader EHeader;
    public ulong NetworkObjectID;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref EHeader);
        serializer.SerializeValue(ref NetworkObjectID);
    }
}
