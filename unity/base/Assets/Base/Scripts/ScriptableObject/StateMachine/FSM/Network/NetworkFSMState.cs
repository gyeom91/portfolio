using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkFSMState", menuName = PATH + "Network/NetworkFSMState")]
public class NetworkFSMState : FSMState, INetworkNode
{
    public virtual void OnServerStarted()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnServerStarted();
        }
    }

    public virtual void OnServerStopped(bool obj)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnServerStopped(obj);
        }
    }

    public virtual void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnConnectionApprovalCallback(request, response);
        }
    }

    public virtual void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnConnectionEvent(arg1, arg2);
        }
    }

    public virtual void OnClientConnectedCallback(ulong obj)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnClientConnectedCallback(obj);
        }
    }

    public virtual void OnClientDisconnectCallback(ulong obj)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnClientDisconnectCallback(obj);
        }
    }

    public virtual void OnClientStarted()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnClientStarted();
        }
    }

    public virtual void OnClientStopped(bool obj)
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_actions[i] is NetworkFSMAction networkFSMAction)
                networkFSMAction.OnClientStopped(obj);
        }
    }
}
