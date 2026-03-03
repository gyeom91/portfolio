using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobbyCreatePanel : UIPanel
{
    [SerializeField] private TMP_InputField _lobbyNameInputField;
    [SerializeField] private Slider _maxPlayerSlider;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _createBtn;
    private int _maxPlayers = 5;

    protected override void OnShow()
    {
        base.OnShow();

        _lobbyNameInputField.text = string.Empty;
    }

    protected override void OnClickedCloseBtn()
    {
        _uIService.Open<UILobbySearchPanel>();
    }

    protected override void Awake()
    {
        base.Awake();

        _closeBtn.onClick.AddListener(OnClickedCloseBtn);
        _createBtn.onClick.AddListener(OnClickedCreateBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _closeBtn.onClick.RemoveListener(OnClickedCloseBtn);
        _createBtn.onClick.RemoveListener(OnClickedCreateBtn);
    }

    private async void OnClickedCreateBtn()
    {
        if (string.IsNullOrEmpty(_lobbyNameInputField.text))
            return;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        uIService.Open<UIProgressPanel>(false);

        var playerData = Node.Get<AuthenticationData>();
        var options = new CreateLobbyOptions()
        {
            Data = new Dictionary<string, DataObject>()
            {
                { "HostName", new DataObject(DataObject.VisibilityOptions.Public, playerData.PlayerName) }
                ,{ "Scene", new DataObject(DataObject.VisibilityOptions.Public, "SurvivorsLikeBattle") }
                ,{ Constants.RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, string.Empty) }
            },
            Player = new Player()
            {
                Data = new Dictionary<string, PlayerDataObject>()
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerData.PlayerName) }
                    ,{ "CharacterName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerPrefs.GetString("CharacterName", "Archer")) }
                    ,{ "Ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, $"{false}") }
                }
            }
        };

        await LobbyManager.Instance.CreateLobby(_lobbyNameInputField.text, _maxPlayers, options);
        await FSMModule.Trigger(Constants.LOBBY_STAY);
    }
}
