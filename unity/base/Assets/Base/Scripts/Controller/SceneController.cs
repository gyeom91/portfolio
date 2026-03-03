using Unity.Netcode;
using UnityEngine;

public class SceneController : BaseMonobehaviour
{
    public static SceneController Instance
    {
        get
        {
            if (_instance.IsNull())
                _instance = FindAnyObjectByType<SceneController>();

            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    protected static SceneController _instance = null;

    [SerializeField] private NetworkManager _networkManagerPrefab;
    [SerializeField] private Service[] _servicePrefabs;
    [SerializeField] private string _bgmSoundID;
    private Service[] _services = null;

    public T GetService<T>() where T : Service
    {
        if (_services.IsNull())
            return null;

        var length = _services.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_services[i] is T service)
                return service;
        }

        return null;
    }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        CreateNetworkManager();
        CreateServices();
    }

    protected virtual void Start()
    {
        if (string.IsNullOrEmpty(_bgmSoundID) == false)
            return;

        var soundService = GetService<BaseSoundService>();
        //soundService.Play(_bgmSoundID, Global.GetPrefsToFloat("BGM"));
    }

    private void OnApplicationQuit()
    {
        var authenticationData = Node.Get<AuthenticationData>();
        LobbyManager.Instance.LeaveOrDelete(authenticationData.PlayerID);
    }

    protected virtual void OnDestroy()
    {
        _services = null;
    }

    private void CreateServices()
    {
        var length = _servicePrefabs.Length;
        for (var i = 0; i < length; ++i)
            Instantiate(_servicePrefabs[i], transform);

        _services = GetComponentsInChildren<Service>(true);
    }

    private void CreateNetworkManager()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager.IsNull() == false)
            return;

        networkManager = Instantiate(_networkManagerPrefab);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnApplicationStart()
    {
        var textAsset = Resources.Load<TextAsset>("TableContainer");
        TableManager.Instance.LoadTable<LocalizeTable>(textAsset);
    }
}
