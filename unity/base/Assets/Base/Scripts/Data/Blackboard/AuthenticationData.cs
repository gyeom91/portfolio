using UnityEngine;

public class AuthenticationData
{
    public string PlayerID { get; private set; }
    public string PlayerName { get; private set; }

    public void SetPlayerID(string playerID)
    {
        PlayerID = playerID;
    }

    public void SetPlayerName(string playerName)
    {
        PlayerName = playerName;
    }
}
