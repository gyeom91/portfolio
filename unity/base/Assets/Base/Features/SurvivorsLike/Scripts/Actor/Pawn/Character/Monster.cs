using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Monster : BattleCharacter
{
    protected override float _posSpeed => _isCollisionEnter ? 0 : base._posSpeed;
    protected Transform _target { get; private set; }

    [SerializeField] private GameplayEffectSO _hitEffect;
    private Slider _healthSlider = null;
    private bool _isCollisionEnter = false;

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        _target = null;

        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        var minSqrDistance = float.MaxValue;
        battleWorldService.ForeachActive(pawn =>
        {
            if (pawn is Hero hero == false)
                return;

            float sqrDistance = (transform.position - hero.transform.position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                _target = hero.transform;
            }
        });
        if (_target.IsNull())
            return;

        _moveDirection = (_target.position - transform.position).normalized;
    }

    protected override void OnFixedBehaviour()
    {
        if (_networkObject.IsServer() == false)
            return;

        base.OnFixedBehaviour();
    }

    protected override void OnChangedNetworkAttributeList(NetworkListEvent<NetworkAttributeData> networkListEvent, NetworkList<NetworkAttributeData> networkAttributeDatas)
    {
        base.OnChangedNetworkAttributeList(networkListEvent, networkAttributeDatas);

        if (_healthSlider.IsNull())
            return;

        var networkAttributeData = networkListEvent.Value;
        var attributeName = networkAttributeData.AttributeName;
        switch (attributeName.ToString())
        {
            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_MaxHealth:
                {
                    _healthSlider.maxValue = networkAttributeData.CurrentValue;
                }
                break;

            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Health:
                {
                    _healthSlider.value = networkAttributeData.CurrentValue;
                }
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _healthSlider = GetComponentInChildren<Slider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_networkObject.IsServer() == false)
            return;

        if (collision.transform != _target)
            return;

        _isCollisionEnter = true;

        if (_target.TryGetComponent<NetworkAbilitySystemAdapter>(out var networkAbilitySystemAdapter) == false)
            return;

        //networkAbilitySystemAdapter.ApplyGameplayEffect(_hitEffect);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_networkObject.IsServer() == false)
            return;

        if (collision.transform != _target)
            return;

        if (_target.TryGetComponent<NetworkAbilitySystemAdapter>(out var networkAbilitySystemAdapter) == false)
            return;

        //networkAbilitySystemAdapter.ApplyGameplayEffect(_hitEffect);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_networkObject.IsServer() == false)
            return;

        if (collision.transform != _target)
            return;

        _isCollisionEnter = false;
    }
}
