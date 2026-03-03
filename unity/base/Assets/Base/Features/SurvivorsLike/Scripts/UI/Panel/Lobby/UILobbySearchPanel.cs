using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class UILobbySearchPanel : UIPanel
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _refreshBtn;
    [SerializeField] private Button _createBtn;
    [SerializeField] private Transform _lobbyElementParent;
    [SerializeField] private UILobbyElement _uILobbyElementPrefab;
    [SerializeField] private float _searchTime;
    private List<UILobbyElement> _uILobbyElements = new();

    public void Refresh(IReadOnlyList<Lobby> lobbies)
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        var length = _uILobbyElements.Count;
        for (var i = 0; i < length; ++i)
            poolService.Despawn(_uILobbyElements[i]);

        _uILobbyElements.Clear();

        if (lobbies.IsNull() || lobbies.Count == 0)
            return;

        length = lobbies.Count;
        for (var i = 0; i < length; ++i)
        {
            var uISearchLobbyElement = poolService.Spawn(_uILobbyElementPrefab, _lobbyElementParent);
            uISearchLobbyElement.Setup(lobbies[i]);

            _uILobbyElements.Add(uISearchLobbyElement);
        }
    }

    protected override async void OnClickedCloseBtn()
    {
        base.OnClickedCloseBtn();

        await FSMModule.Trigger(Constants.LOBBY_MAIN);
    }

    protected override void Awake()
    {
        base.Awake();

        _closeBtn.onClick.AddListener(OnClickedCloseBtn);
        _refreshBtn.onClick.AddListener(OnClickedRefreshBtn);
        _createBtn.onClick.AddListener(OnClickedCreateBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _closeBtn.onClick.RemoveListener(OnClickedCloseBtn);
        _refreshBtn.onClick.RemoveListener(OnClickedRefreshBtn);
        _createBtn.onClick.RemoveListener(OnClickedCreateBtn);
    }

    private async void OnClickedRefreshBtn()
    {
        await FSMModule.Trigger(Constants.LOBBY_SEARCH);
    }

    private void OnClickedCreateBtn()
    {
        _uIService.Open<UILobbyCreatePanel>();
    }
}
