using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyStayPanel : UIPanel
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Transform _lobbyPlayerElementParent;
    [SerializeField] private UILobbyPlayerElement _uILobbyPlayerElementPrefab;
    [SerializeField] private UILobbyCharacterSelectToggle[] _toggles;
    [SerializeField] private Button _startBtn;
    private List<UILobbyPlayerElement> _uILobbyPlayerElements = new();

    public void Refresh()
    {
        var lobby = LobbyManager.Instance.Lobby;
        var sceneController = SceneController.Instance as LobbySceneController;
        sceneController.DespawnPlayerCharacter();

        var poolService = sceneController.GetService<PoolService>();
        var length = _uILobbyPlayerElements.Count;
        for (var i = 0; i < length; ++i)
            poolService.Despawn(_uILobbyPlayerElements[i]);

        _uILobbyPlayerElements.Clear();

        var players = lobby.Players;
        var count = players.Count;
        for (var i = 0; i < count; ++i)
        {
            var player = players[i];
            var playerData = player.Data;
            if (playerData.IsNull()
                || playerData.TryGetValue("CharacterName", out var characterNameData) == false
                || playerData.TryGetValue("PlayerName", out var playerNameData) == false
                || playerData.TryGetValue("Ready", out var readyData) == false)
                continue;

            var uILobbyPlayerElement = poolService.Spawn(_uILobbyPlayerElementPrefab, _lobbyPlayerElementParent);
            uILobbyPlayerElement.Setup(playerNameData.Value, i, readyData.Value);
            _uILobbyPlayerElements.Add(uILobbyPlayerElement);

            sceneController.SpawnPlayerCharacter(player.Id, playerNameData.Value, i, $"Lobby_{characterNameData.Value}");
        }

        var authenticationData = Node.Get<AuthenticationData>();
        var isHost = LobbyManager.Instance.IsHost(authenticationData.PlayerID);
        var unreadyPlayers = players.FindAll(player =>
        {
            var playerData = player.Data;
            var readyData = playerData["Ready"];
            return bool.Parse(readyData.Value) == false;
        });
        _startBtn.gameObject.SetActive(isHost && unreadyPlayers.IsNullOrEmpty());
    }

    protected override void OnShow()
    {
        base.OnShow();

        Refresh();

        var toggle = Array.Find(_toggles, toggle => toggle.CharacterName == PlayerPrefs.GetString("CharacterName", "Archer"));
        if (toggle.IsNull())
            return;

        toggle.IsOn = true;
    }

    protected override async void OnClickedCloseBtn()
    {
        base.OnClickedCloseBtn();

        await FSMModule.Trigger(Constants.LOBBY_SEARCH);
    }

    protected override void Awake()
    {
        base.Awake();

        LobbyManager.Instance.OnLobbyChanged += OnLobbyChanged;

        _closeBtn.onClick.AddListener(OnClickedCloseBtn);
        _startBtn.onClick.AddListener(OnClickedStartBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        LobbyManager.Instance.OnLobbyChanged -= OnLobbyChanged;

        _closeBtn.onClick.RemoveListener(OnClickedCloseBtn);
        _startBtn.onClick.RemoveListener(OnClickedStartBtn);
    }

    private void OnLobbyChanged(Unity.Services.Lobbies.ILobbyChanges lobbyChanges)
    {
        Refresh();
    }

    private async void OnClickedStartBtn()
    {
        await FSMModule.Trigger(Constants.NETWORK_HOST_ONLINE);
    }
}
