using Unity.Netcode;
using UnityEngine;

public class BattleSceneController : SceneController
{
    public bool Freeze { get; protected set; } = true;
    public int MaxExp { get; private set; } = 0;
    public int CurrentExp { get; private set; } = 0;

    [SerializeField] private string[] _monsterNames;
    [SerializeField] private float _waitForSeconds;
    [SerializeField] private float _spawnTime;
    private float _spawnTimer = 0;
    //private LevelingData _levelingData;

    public void SetFreeze(bool freeze)
    {
        Freeze = freeze;
    }

    public bool IsReadyToLevelUp(int value)
    {
        if ((CurrentExp + value) < MaxExp)
        {
            CurrentExp += value;
            return false;
        }

        CurrentExp = MaxExp - (CurrentExp + value);
        //MaxExp = _levelingData.Exp * _levelingData.Rate;
        return true;
    }

    protected override void Awake()
    {
        base.Awake();

        var networkService = GetService<NetworkService>();
        networkService.RegisterEvents(Constants.EHeader.LOAD_COMPLETED, OnLoadEventCompleted);

        var uIService = GetService<UIService>();
        uIService.Open<UIProgressPanel>();


        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsNull() || networkManager.IsServer == false)
            return;

        var clients = networkManager.ConnectedClientsList;
        var count = clients.Count;
        //var levelingTable = TableManager.Instance.GetTable<LevelingTable>();
        //if (levelingTable.TryGetData(count, out var levelingData) == false)
        //    return;

        //_levelingData = levelingData;
        //MaxExp = _levelingData.Exp;
    }

    protected override void OnDestroy()
    {
        var networkService = GetService<NetworkService>();
        networkService.UnregisterEvents(Constants.EHeader.LOAD_COMPLETED, OnLoadEventCompleted);

        base.OnDestroy();
    }

    protected virtual async void OnLoadEventCompleted(Constants.EHeader eHeader, FastBufferReader reader)
    {
        Freeze = true;

        await Awaitable.WaitForSecondsAsync(_waitForSeconds);

        Freeze = false;

        await FSMModule.Trigger(Constants.BATTLE_MAIN);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            await FSMModule.Trigger(Constants.BATTLE_OPTION);

        if (Freeze)
            return;

        if (_monsterNames.IsNullOrEmpty())
            return;

        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsNull() || networkManager.IsServer == false)
            return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer < _spawnTime)
            return;

        _spawnTimer = 0;

        var spawnManager = networkManager.SpawnManager;
        var poolService = GetService<NetworkPoolService>();
        var monsterName = _monsterNames.GetRandomValue();
        var battleWorldService = GetService<BattleWorldService>();
        var randomPos = battleWorldService.GetRandomPosition();
        poolService.HandlerSpawn(monsterName, new EmptySyncData(), randomPos, Quaternion.identity);
    }
}
