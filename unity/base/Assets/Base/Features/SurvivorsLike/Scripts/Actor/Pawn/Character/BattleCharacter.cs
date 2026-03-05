using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkSpawnable))]
[RequireComponent(typeof(NetworkAbilitySystemAdapter))]
public class BattleCharacter : Character, INetworkInitialize, INetworkRelease
{
    public NetworkAbilitySystemAdapter Adapter { get; private set; }

    protected override float _posSpeed => Adapter.MoveSpeed;
    protected NetworkObject _networkObject { get; private set; }

    public virtual void OnInitialize(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.AddWorldPawn(this);

        var asc = Adapter.ASC;
        asc.AddAttributeSet(new CharacterAttributeSet());
    }

    public virtual void OnRelease()
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.RemoveWorldPawn(this);
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        Adapter.Tick();
    }

    protected override void Awake()
    {
        base.Awake();

        Adapter = GetComponent<NetworkAbilitySystemAdapter>();
        Adapter.InitializeAbilitySystem(this, gameObject);

        _networkObject = GetComponent<NetworkObject>();
    }
}
