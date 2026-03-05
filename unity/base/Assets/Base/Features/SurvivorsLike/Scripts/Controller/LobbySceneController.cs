using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbySceneController : SceneController
{
    public LobbyActor LocalPlayer { get; private set; }

    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Vector3 _spawnRotation;
    private List<(string, int, string, LobbyActor)> _players = new();

    public void DespawnAllPlayerCharacter()
    {
        var poolService = GetService<PoolService>();
        var cinemachineService = GetService<CinemachineService>();
        var groupCinemachineHandler = cinemachineService.GetHandler<GroupCinemachineHandler>();
        var length = _players.Count;
        for (var i = 0; i < length; ++i)
        {
            var player = _players[i];
            var playerID = player.Item1;
            var playerIndex = player.Item2;
            var characterName = player.Item3;
            var character = player.Item4;
            groupCinemachineHandler.RemoveTarget(character.transform);

            poolService.Despawn(character);
        }

        _players.Clear();
    }

    public async void SpawnPlayerCharacter(string playerID, string playerName, int playerIndex, string characterName)
    {
        var poolService = GetService<PoolService>();
        var spawnPosition = _spawnPosition + Vector3.right * (playerIndex * 2);
        var lobbyActor = await poolService.SpawnAsync<LobbyActor>(characterName, position: spawnPosition, rotation: Quaternion.Euler(_spawnRotation));
        lobbyActor.Setup(playerName);
        lobbyActor.SetEnableCanvas(true);

        _players.Add((playerID, playerIndex, characterName, lobbyActor));

        var cinemachineService = GetService<CinemachineService>();
        var groupCinemachineHandler = cinemachineService.GetHandler<GroupCinemachineHandler>();
        groupCinemachineHandler.AddTarget(lobbyActor.transform);

        var authenticationData = Node.Get<AuthenticationData>();
        if (authenticationData.PlayerID != playerID)
            return;

        cinemachineService.SetFollowTarget(lobbyActor.transform);
        cinemachineService.SetLookAtTarget(lobbyActor.transform);
        LocalPlayer = lobbyActor;
    }

    protected override void Awake()
    {
        base.Awake();

        LobbyManager.Instance.OnLobbyChanged += OnLobbyChanged;
        LobbyManager.Instance.OnLobbyDeleted += OnLobbyDeleted;
        LobbyManager.Instance.OnKickedFromLobby += OnKickedFromLobby;
    }

    protected override async void Start()
    {
        base.Start();

        await FSMModule.Trigger(Constants.NETWORK_LOBBY);

        var authenticationData = Node.Get<AuthenticationData>();
        SpawnPlayerCharacter(authenticationData.PlayerID, authenticationData.PlayerName, 0, $"Lobby_{PlayerPrefs.GetString("CharacterName", "Archer")}");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        LobbyManager.Instance.OnLobbyChanged -= OnLobbyChanged;
        LobbyManager.Instance.OnLobbyDeleted -= OnLobbyDeleted;
        LobbyManager.Instance.OnKickedFromLobby -= OnKickedFromLobby;
    }

    private async void OnLobbyChanged(ILobbyChanges lobbyChanges)
    {
        var changedOrRemovedLobbyValue = lobbyChanges.Data;
        if (changedOrRemovedLobbyValue.IsNull())
            return;

        var authenticationData = Node.Get<AuthenticationData>();
        if (LobbyManager.Instance.IsHost(authenticationData.PlayerID))
            return;

        var dataDic = changedOrRemovedLobbyValue.Value;
        if (dataDic.IsNull())
            return;

        if (dataDic.TryGetValue(Constants.RELAY_CODE, out var output) == false)
            return;

        var dataObject = output.Value;
        if (dataObject.IsNull() || string.IsNullOrEmpty(dataObject.Value))
            return;

        await FSMModule.Trigger(Constants.NETWORK_CLIENT);
    }

    private void OnLobbyDeleted()
    {
        var uIService = GetService<UIService>();
        var uIMessagePanel = uIService.Get<UIMessagePanel>();
        var message = "방장나감!";
        uIMessagePanel.OnConfirmClick += async () =>
        {
            LobbyManager.Instance.Clear();

            await FSMModule.Trigger(Constants.LOBBY_MAIN);
        };
        uIMessagePanel.ShowMessage(UIMessagePanel.ETitle.Error, UIMessagePanel.EMessage.Confirm, message);
    }

    private void OnKickedFromLobby()
    {
        var uIService = GetService<UIService>();
        var uIMessagePanel = uIService.Get<UIMessagePanel>();
        var message = "강퇴당함!";
        uIMessagePanel.OnConfirmClick += async () =>
        {
            LobbyManager.Instance.Clear();

            await FSMModule.Trigger(Constants.LOBBY_MAIN);
        };
        uIMessagePanel.ShowMessage(UIMessagePanel.ETitle.Error, UIMessagePanel.EMessage.Confirm, message);
    }
}
