using UnityEngine;

public class UILobbyCharacterSelectToggle : UIToggle
{
    public string CharacterName { get => _characterName; }

    [SerializeField] private string _characterName;

    protected override async void OnChangedValueToggle(bool isOn)
    {
        if (isOn == false)
            return;

        var authenticationData = Node.Get<AuthenticationData>();
        await LobbyManager.Instance.UpdatePlayerData(authenticationData.PlayerID, "CharacterName", _characterName);
    }
}
