using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobbyElement : UI
{
    [SerializeField] private TextMeshProUGUI _lengthTMPro;
    [SerializeField] private TextMeshProUGUI _lobbyNameTMPro;
    [SerializeField] private TextMeshProUGUI _hostNameTMPro;
    private Button _btn = null;
    private Lobby _lobby = null;

    public void Setup(Lobby lobby)
    {
        _lobby = lobby;
        _lengthTMPro.text = $"{_lobby.Players.Count}/{_lobby.MaxPlayers}";
        _lobbyNameTMPro.text = $"{_lobby.Name}";

        var lobbyData = _lobby.Data;
        if (lobbyData.IsNull())
            return;

        if (lobbyData.TryGetValue("HostName", out var output) == false)
            return;

        _hostNameTMPro.text = $"{output.Value}";
    }

    protected override void Awake()
    {
        base.Awake();

        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickedBtn);
    }

    protected virtual void OnDestroy()
    {
        _btn.onClick.RemoveListener(OnClickedBtn);
    }

    private async void OnClickedBtn()
    {
        if (_lobby.IsLocked)
            return;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        var authenticationData = Node.Get<AuthenticationData>();
        var joinLobbyByIdOptions = new JoinLobbyByIdOptions()
        {
            Player = new Player()
            {
                Data = new System.Collections.Generic.Dictionary<string, PlayerDataObject>()
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, authenticationData.PlayerName) }
                    ,{ "CharacterName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerPrefs.GetString("CharacterName", "Archer")) }
                    ,{ "Ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, $"{false}") }
                }
            }
        };

        await LobbyManager.Instance.JoinLobby(_lobby, joinLobbyByIdOptions);
        await FSMModule.Trigger(Constants.LOBBY_STAY);
    }
}
