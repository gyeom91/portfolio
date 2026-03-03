using UnityEngine;

public class UICharacterSelectToggle : UIToggle
{
    public string CharacterName { get => _characterName; }

    [SerializeField] private string _characterName;

    protected override void OnChangedValueToggle(bool isOn)
    {
        if (isOn == false)
            return;

        if (_characterName == PlayerPrefs.GetString("CharacterName", "Archer"))
            return;

        PlayerPrefs.SetString("CharacterName", _characterName);
        PlayerPrefs.Save();

        var sceneController = SceneController.Instance as LobbySceneController;
        var authenticationData = Node.Get<AuthenticationData>();
        sceneController.DespawnPlayerCharacter();
        sceneController.SpawnPlayerCharacter(authenticationData.PlayerID, authenticationData.PlayerName, 0, $"Lobby_{_characterName}");
    }
}
