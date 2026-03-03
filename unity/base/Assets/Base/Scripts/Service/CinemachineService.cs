using Unity.Cinemachine;
using UnityEngine;

public class CinemachineService : Service
{
    [SerializeField] private GameObject _lookAtPrefab;
    [SerializeField] private CinemachineHandler[] _handlerPrefabs;
    private Transform _lookAt = null;
    private Camera _mainCamera = null;
    private CinemachineBrain _brain = null;
    private CinemachineHandler[] _handlers = null;

    public void SetFollowTarget(Transform target)
    {
        var length = _handlers.Length;
        for (var i = 0; i < length; ++i)
            _handlers[i].SetFollowTarget(target);
    }

    public void SetLookAtTarget(Transform target)
    {
        var length = _handlers.Length;
        for (var i = 0; i < length; ++i)
            _handlers[i].SetLookAtTarget(target);
    }

    public void CreateLookAtController()
    {
        if (_lookAt.IsNull() == false)
            return;

        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();
        var clone = poolService.Spawn(_lookAtPrefab);
        _lookAt = clone.transform;

        SetFollowTarget(_lookAt);
        SetLookAtTarget(_lookAt);
    }

    public void LookAtPosition(Vector3 position)
    {
        _lookAt.position = position;
    }

    public async void LookAtLerpPosition(Vector3 position)
    {
        while (Vector3.Distance(position, _lookAt.position) >= 0)
        {
            _lookAt.position = Vector3.Lerp(position, _lookAt.position, Time.deltaTime);

            await Awaitable.NextFrameAsync();
        }

        _lookAt.position = position;
    }

    public T GetHandler<T>() where T : CinemachineHandler
    {
        var length = _handlers.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_handlers[i] is T handler)
                return handler;
        }

        return null;
    }

    public void ChangeHandler<T>(T nextHandler = null) where T : CinemachineHandler
    {
        var length = _handlers.Length;
        for (var i = 0; i < length; ++i)
        {
            var handler = _handlers[i];
            if (nextHandler.IsNull() == false)
            {
                if (handler.GetType() == nextHandler.GetType())
                {
                    handler.SetPriority(1);
                }
                else
                {
                    handler.SetPriority(0);
                }
            }
            else
            {
                if (handler is T)
                {
                    handler.SetPriority(1);
                }
                else
                {
                    handler.SetPriority(0);
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
        _brain = _mainCamera.GetComponent<CinemachineBrain>();

        var length = _handlerPrefabs.Length;
        for (var i = 0; i < length; ++i)
            Instantiate(_handlerPrefabs[i], transform);

        _handlers = GetComponentsInChildren<CinemachineHandler>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _mainCamera = null;
        _brain = null;
        _handlers = null;
    }
}
