using Unity.Netcode;
using UnityEngine;

public class NetworkSpawnable : BaseNetworkBehaviour
{
    private INetworkInitialize[] _networkInitializes = null;
    private INetworkRelease[] _networkReleases = null;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (_networkInitializes.IsNullOrEmpty())
            return;

        var length = _networkInitializes.Length;
        for (var i = 0; i < length; ++i)
        {
            _networkInitializes[i].OnInitialize(NetworkObject);
        }
    }

    public override void OnNetworkPreDespawn()
    {
        base.OnNetworkPreDespawn();

        if (_networkReleases.IsNullOrEmpty())
            return;

        var length = _networkReleases.Length;
        for (var i = 0; i < length; ++i)
        {
            _networkReleases[i].OnPrevRelease();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (_networkReleases.IsNullOrEmpty())
            return;

        var length = _networkReleases.Length;
        for (var i = 0; i < length; ++i)
        {
            _networkReleases[i].OnRelease();
        }
    }

    protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
    {
        base.OnNetworkPreSpawn(ref networkManager);

        if (_networkInitializes.IsNullOrEmpty())
            return;

        var length = _networkInitializes.Length;
        for (var i = 0; i < length; ++i)
        {
            _networkInitializes[i].OnPrevInitialize(networkManager);
        }
    }

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();

        if (_networkInitializes.IsNullOrEmpty())
            return;

        var length = _networkInitializes.Length;
        for (var i = 0; i < length; ++i)
        {
            _networkInitializes[i].OnPostInitialize();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _networkInitializes = GetComponentsInChildren<INetworkInitialize>();
        _networkReleases = GetComponentsInChildren<INetworkRelease>();
    }
}
