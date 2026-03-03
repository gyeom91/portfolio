using System;
using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkSpawnable))]
public class BattleCharacter : Character, INetworkInitialize, INetworkRelease
{
    public AbilitySystemComponent ASC { get; private set; }

    protected override float _posSpeed => _speed;
    protected NetworkObject _networkObject { get; private set; }

    private float _speed = 0;

    public virtual void OnInitialize(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.AddWorldPawn(this);
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        ASC.Tick(Time.deltaTime, _networkObject.IsOwnedByServer);
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

        var effectContextFactory = new GameplayEffectContextFactory();
        ASC = new AbilitySystemComponent(effectContextFactory);
    }

    protected virtual void OnDestroy()
    {
        ASC.Dispose();
    }
}
