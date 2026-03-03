using Unity.Netcode;

public interface INetworkNode
{
    public void OnServerStarted();

    public void OnServerStopped(bool obj);

    public void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response);

    public void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2);

    public void OnClientConnectedCallback(ulong obj);

    public void OnClientDisconnectCallback(ulong obj);

    public void OnClientStarted();

    public void OnClientStopped(bool obj);
}
