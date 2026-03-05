using Unity.Netcode;
using UnityEngine;

public interface INetworkInitialize
{
    void OnInitialize(NetworkObject networkObject);
}
