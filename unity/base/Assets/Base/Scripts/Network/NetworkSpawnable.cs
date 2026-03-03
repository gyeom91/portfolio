using UnityEngine;

public class NetworkSpawnable : BaseNetworkBehaviour
{
    private INetworkInitialize[] _networkInitializes = null;
    private INetworkRelease[] _networkReleases = null;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (_networkInitializes.IsNull())
            return;

        var length = _networkInitializes.Length;
        for (var i = 0; i < length; ++i)
            _networkInitializes[i].OnInitialize(NetworkObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (_networkReleases.IsNull())
            return;

        var length = _networkReleases.Length;
        for (var i = 0; i < length; ++i)
            _networkReleases[i].OnRelease();
    }

    protected override void Awake()
    {
        base.Awake();

        _networkInitializes = GetComponentsInChildren<INetworkInitialize>();
        _networkReleases = GetComponentsInChildren<INetworkRelease>();
    }
}
