using Unity.Netcode;
using UnityEngine;

public class NetworkFSMModule : FSMModule, INetworkNode
{
    public void OnServerStarted()
    {
        ConvertNetworkState()?.OnServerStarted();
    }

    public void OnServerStopped(bool obj)
    {
        ConvertNetworkState()?.OnServerStopped(obj);
    }

    public void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        ConvertNetworkState()?.OnConnectionApprovalCallback(request, response);
    }

    public void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
    {
        ConvertNetworkState()?.OnConnectionEvent(arg1, arg2);
    }

    public void OnClientConnectedCallback(ulong obj)
    {
        ConvertNetworkState()?.OnClientConnectedCallback(obj);
    }

    public void OnClientDisconnectCallback(ulong obj)
    {
        ConvertNetworkState()?.OnClientDisconnectCallback(obj);
    }

    public void OnClientStarted()
    {
        ConvertNetworkState()?.OnClientStarted();
    }

    public void OnClientStopped(bool obj)
    {
        ConvertNetworkState()?.OnClientStopped(obj);
    }

    protected override async Awaitable Enter()
    {
        await base.Enter();

        var networkManager = NetworkManager.Singleton;
        OnInstantiated(networkManager);

        NetworkManager.OnDestroying += OnDestroying;
    }

    protected override async Awaitable Exit()
    {
        await base.Exit();

        NetworkManager.OnDestroying -= OnDestroying;
    }

    private NetworkFSMState ConvertNetworkState()
    {
        return _currentState as NetworkFSMState;
    }

    private void OnInstantiated(NetworkManager networkManager)
    {
        networkManager.OnServerStarted += OnServerStarted;
        networkManager.OnServerStopped += OnServerStopped;
        networkManager.ConnectionApprovalCallback += OnConnectionApprovalCallback;
        networkManager.OnConnectionEvent += OnConnectionEvent;
        networkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        networkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
        networkManager.OnClientStarted += OnClientStarted;
        networkManager.OnClientStopped += OnClientStopped;
    }

    private void OnDestroying(NetworkManager networkManager)
    {
        networkManager.OnServerStarted -= OnServerStarted;
        networkManager.OnServerStopped -= OnServerStopped;
        networkManager.ConnectionApprovalCallback -= OnConnectionApprovalCallback;
        networkManager.OnConnectionEvent -= OnConnectionEvent;
        networkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        networkManager.OnClientStarted -= OnClientStarted;
        networkManager.OnClientStopped -= OnClientStopped;
    }
}
