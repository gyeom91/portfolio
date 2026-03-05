using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class Arrow : Pawn, IInstanceData, INetworkInitialize, INetworkRelease
{
    [SerializeField] private GameplayEffectSO _hitEffect;
    private NetworkObject _networkObject = null;
    private float _damage = 0;
    private float _speed = 0;

    public void SetInstantiationData<T>(NetworkManager networkManager, T instanceData) where T : struct, INetworkSerializable
    {
        if (instanceData is ArrowSyncData arrowSyncData == false)
            return;

        var networkObject = networkManager.GetNetworkObject(arrowSyncData.NetworkID);
        if (networkObject.TryGetComponent<BattleCharacter>(out var character) == false)
            return;

        var adapter = character.Adapter;
        var asc = adapter.ASC;
        var damageAttribute = asc.GetAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Damage);
        _damage = damageAttribute.CurrentValue;

        var speedAttribute = asc.GetAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Ability_Speed);
        _speed = speedAttribute.CurrentValue;
    }

    public void OnInitialize(NetworkObject networkObject)
    {
        _networkObject = networkObject;

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

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_networkObject.IsSpawned == false || _networkObject.IsServer() == false)
            return;

        if (other.TryGetComponent<IWorldActor>(out var actor) == false)
            return;

        if (actor is Environment environment)
        {
            _networkObject.Despawn();
        }
        else if (actor is Monster monster)
        {
            var adapter = monster.Adapter;
            adapter.ApplyGameplayEffect(_hitEffect);

            _networkObject.Despawn();
        }
        else if (actor is Hero hero)
        {

        }
        else
        {
            // other is arrow
        }
    }
}
