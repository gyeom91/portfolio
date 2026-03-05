using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkSpawnable))]
[RequireComponent(typeof(NetworkAbilitySystemAdapter))]
public class BattleCharacter : Character, INetworkInitialize, INetworkRelease
{
    public NetworkAbilitySystemAdapter Adapter { get; private set; }

    protected override float _posSpeed => _moveSpeed;
    protected NetworkObject _networkObject { get; private set; }

    private float _moveSpeed = 0;

    public virtual void OnInitialize(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.AddWorldPawn(this);

        var asc = Adapter.ASC;
        asc.AddAttributeSet(new CharacterAttributeSet());

        Adapter.OnChangedNetworkAttributeList += OnChangedNetworkAttributeList;
    }

    public virtual void OnRelease()
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.RemoveWorldPawn(this);

        Adapter.OnChangedNetworkAttributeList -= OnChangedNetworkAttributeList;
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        Adapter.Tick();
    }

    protected virtual void OnChangedNetworkAttributeList(NetworkListEvent<NetworkAttributeData> networkListEvent, NetworkList<NetworkAttributeData> networkAttributeDatas)
    {
        if (_networkObject.IsOwner == false)
            return;

        var networkAttributeData = networkListEvent.Value;
        var attributeName = networkAttributeData.AttributeName;
        switch (attributeName.ToString())
        {
            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Speed:
                {
                    _moveSpeed = networkAttributeData.CurrentValue;
                }
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Adapter = GetComponent<NetworkAbilitySystemAdapter>();
        Adapter.InitializeAbilitySystem(this, gameObject);

        _networkObject = GetComponent<NetworkObject>();
    }
}
