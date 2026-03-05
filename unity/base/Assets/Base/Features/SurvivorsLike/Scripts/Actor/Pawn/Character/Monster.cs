using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Monster : BattleCharacter
{
    protected Transform _target { get; private set; }

    private Slider _healthSlider = null;

    public override void OnInitialize(NetworkObject networkObject)
    {
        base.OnInitialize(networkObject);

        Adapter.OnChangedNetworkAttributeList += OnChangedNetworkAttributeList;
    }

    public override void OnRelease()
    {
        base.OnRelease();

        Adapter.OnChangedNetworkAttributeList -= OnChangedNetworkAttributeList;
    }

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

    protected override void Awake()
    {
        base.Awake();

        _healthSlider = GetComponentInChildren<Slider>();
    }

    private void OnChangedNetworkAttributeList(NetworkListEvent<NetworkAttributeData> networkListEvent, NetworkList<NetworkAttributeData> networkAttributeDatas)
    {
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
}
