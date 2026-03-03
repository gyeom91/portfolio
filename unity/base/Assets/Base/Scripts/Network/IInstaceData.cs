using Unity.Netcode;
using UnityEngine;

public interface IInstanceData
{
    void SetInstantiationData<T>(NetworkManager networkManager, T instanceData) where T : struct, INetworkSerializable;
}
