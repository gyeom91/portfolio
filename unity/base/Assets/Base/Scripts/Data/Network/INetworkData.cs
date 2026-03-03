using Unity.Netcode;
using UnityEngine;

public interface INetworkData : INetworkSerializable
{
    public ulong NetworkObjectID { get; }
}
