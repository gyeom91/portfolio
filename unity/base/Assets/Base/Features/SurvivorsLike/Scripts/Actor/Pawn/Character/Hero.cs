using System.Collections.Generic;
using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class Hero : BattleCharacter
{
    [SerializeField, AttributeNameSelector] private List<string> _abilityTags;
    [SerializeField] private float _noiseAmplitude;
    [SerializeField] private float _noiseDuration;

    public override void OnInitialize(NetworkObject networkObject)
    {
        base.OnInitialize(networkObject);

        var asc = Adapter.ASC;
        asc.AddAttributeSet(new HeroAttributeSet(CharacterName));

        if (networkObject.IsOwner == false)
            return;

        var sceneController = SceneController.Instance;
        var cinemachineService = sceneController.GetService<CinemachineService>();
        var noiseCinemachineHandler = cinemachineService.GetHandler<NoiseCinemachineHandler>();
        noiseCinemachineHandler.SetFollowTarget(transform);
        noiseCinemachineHandler.SetLookAtTarget(transform);

        cinemachineService.ChangeHandler(noiseCinemachineHandler);
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        if (_networkObject.IsOwner == false)
            return;

        _moveDirection = InputService.MoveDirection;
    }

    protected override void OnFixedBehaviour()
    {
        if (_networkObject.IsOwner == false)
            return;

        base.OnFixedBehaviour();
    }

    protected override void OnChangedNetworkAttributeList(NetworkListEvent<NetworkAttributeData> networkListEvent, NetworkList<NetworkAttributeData> networkAttributeDatas)
    {
        base.OnChangedNetworkAttributeList(networkListEvent, networkAttributeDatas);

        if (_networkObject.IsOwner == false)
            return;

        var sceneController = SceneController.Instance;
        var uIService = sceneController.GetService<UIService>();
        var uIBattlePanel = uIService.Get<UIBattlePanel>();
        var networkAttributeData = networkListEvent.Value;
        var attributeName = networkAttributeData.AttributeName;
        switch (attributeName.ToString())
        {
            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_MaxHealth:
                {
                    uIBattlePanel.SetMaxHealth(networkAttributeData.CurrentValue);
                }
                break;

            case SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Health:
                {
                    uIBattlePanel.SetHealth(networkAttributeData.CurrentValue);
                }
                break;
        }
    }

    private void Update()
    {
        if (SceneController.Instance is BattleSceneController sceneController == false || sceneController.Freeze)
            return;

        OnBehaviour();

        if (_abilityTags.IsNullOrEmpty())
            return;

        var length = _abilityTags.Count;
        for (var i = 0; i < length; ++i)
        {
            Adapter.TryActivateAbilityWithTag(_abilityTags[i]);
        }
    }

    private void FixedUpdate()
    {
        if (SceneController.Instance is BattleSceneController sceneController == false || sceneController.Freeze)
            return;

        OnFixedBehaviour();
    }
}
