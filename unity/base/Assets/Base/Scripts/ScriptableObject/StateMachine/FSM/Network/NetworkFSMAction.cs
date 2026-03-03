using Unity.Netcode;
using UnityEngine;

public class NetworkFSMAction : FSMAction, INetworkNode
{
    public virtual void OnServerStarted() { }

    public virtual void OnServerStopped(bool obj) { }

    public virtual void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response) { }

    public virtual void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2) { }

    public virtual void OnClientConnectedCallback(ulong obj) { }

    public virtual void OnClientDisconnectCallback(ulong obj) { }

    public virtual void OnClientStarted() { }

    public virtual void OnClientStopped(bool obj) { }
}
