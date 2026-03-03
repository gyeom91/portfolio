using System;

[Serializable]
public struct NetworkConnectionPayload
{
    public string PlayerID;
    public string PlayerName;

    public NetworkConnectionPayload(NetworkConnectedSyncData networkConnectedSyncData)
    {
        PlayerID = networkConnectedSyncData.PlayerID.ToString();
        PlayerName = networkConnectedSyncData.PlayerName.ToString();
    }
}
