using UnityEngine;

public class UILobbyReadyToggle : UIToggle
{
    protected override async void OnChangedValueToggle(bool isOn)
    {
        var authenticationData = Node.Get<AuthenticationData>();
        await LobbyManager.Instance.UpdatePlayerData(authenticationData.PlayerID, "Ready", $"{isOn}");
    }
}
