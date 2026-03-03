using UnityEngine;

public class UnitySingleton<T> : BaseMonobehaviour where T : BaseMonobehaviour
{
    public static T Instance { get => _instance; }

    private static T _instance = null;

    protected virtual void Initialize()
    {
        _instance = this as T;

        DontDestroyOnLoad(gameObject);
    }

    protected virtual void Release()
    {
        _instance = null;
    }

    protected override void Awake()
    {
        base.Awake();

        if (_instance.IsNull())
        {
            Initialize();
        }
        else
        {
            if (_instance.GetInstanceID() == GetInstanceID())
                return;

            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        if (_instance.GetInstanceID() != GetInstanceID())
            return;

        Release();
    }
}
