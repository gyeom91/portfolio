using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

//[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
public struct NetworkAttributeData : INetworkSerializable, IEquatable<NetworkAttributeData>
{
    public FixedString128Bytes AttributeName;
    public float BaseValue;
    public float CurrentValue;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref AttributeName);
        serializer.SerializeValue(ref BaseValue);
        serializer.SerializeValue(ref CurrentValue);
    }

    public bool Equals(NetworkAttributeData other)
    {
        return AttributeName.Equals(other.AttributeName);
    }
}
