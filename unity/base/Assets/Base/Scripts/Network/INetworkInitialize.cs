using Unity.Netcode;
using UnityEngine;

public interface INetworkInitialize
{
    void OnPrevInitialize(NetworkManager networkManager);
    void OnInitialize(NetworkObject networkObject);
    void OnPostInitialize();
}
