using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkSpawnable))]
public class BattleCharacter : Character, INetworkInitialize, INetworkRelease
{
    protected override float _posSpeed => _speed;
    protected NetworkObject _networkObject { get; private set; }

    private float _speed = 0;

    public virtual void OnInitialize(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.AddWorldPawn(this);
    }

    public virtual void OnRelease()
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.RemoveWorldPawn(this);
    }

    protected override void Awake()
    {
        base.Awake();

        _networkObject = GetComponent<NetworkObject>();
    }
}
