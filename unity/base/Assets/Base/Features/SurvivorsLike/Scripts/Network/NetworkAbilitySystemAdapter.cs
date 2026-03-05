using System;
using System.Collections.Generic;
using CycloneGames.GameplayTags.Runtime;
using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class NetworkAbilitySystemAdapter : BaseNetworkBehaviour
{
    public event Action<NetworkListEvent<NetworkAttributeData>, NetworkList<NetworkAttributeData>> OnChangedNetworkAttributeList;
    public AbilitySystemComponent ASC { get; private set; }
    public float MoveSpeed { get; private set; }

    protected Dictionary<string, Action<float, float>> _changeCallbackContainer { get; private set; } = new();

    [SerializeField, AttributeNameSelector] private List<string> _initializeAttributes;
    [SerializeField] private List<GameplayEffectSO> _initializeEffects;
    [SerializeField] private List<GameplayAbilitySO> _initializeAbilities;
    private NetworkList<NetworkAttributeData> _networkAttributeDatas = new();

    public void TryActivateAbilityWithTag(GameplayTag gameplayTag)
    {
        if (IsServer == false)
            return;

        var abilities = ASC.GetActivatableAbilities();
        if (abilities.IsNullOrEmpty())
            return;

        foreach (var abilitySpec in abilities)
        {
            if (abilitySpec.Ability != null && abilitySpec.Ability.AbilityTags.HasTag(gameplayTag) == false)
                continue;

            ASC.TryActivateAbility(abilitySpec);
        }
    }

    public void ApplyGameplayEffect(GameplayEffectSO gameplayEffectSO)
    {
        if (IsServer == false)
            return;

        var ge = gameplayEffectSO.GetGameplayEffect();
        var spec = GameplayEffectSpec.Create(ge, ASC);
        ASC.ApplyGameplayEffectSpecToSelf(spec);
    }

    public void SetAttributeValue(string attributeName, GameplayAttribute gameplayAttribute)
    {
        if (IsServer == false)
            return;

        var newAttributeData = new NetworkAttributeData();
        newAttributeData.AttributeName = attributeName;
        newAttributeData.BaseValue = gameplayAttribute.BaseValue;
        newAttributeData.CurrentValue = gameplayAttribute.CurrentValue;

        var index = _networkAttributeDatas.IndexOf(newAttributeData);
        if (index == -1)
        {
            _networkAttributeDatas.Add(newAttributeData);
        }
        else
        {
            _networkAttributeDatas.Set(index, newAttributeData, true);
        }
    }

    public void Tick()
    {
        ASC.Tick(Time.deltaTime, IsServer);
    }

    public void InitializeAbilitySystem(BaseMonobehaviour monobehaviour, GameObject gameObject)
    {
        var effectContextFactory = new GameplayEffectContextFactory();
        ASC = new AbilitySystemComponent(effectContextFactory);
        ASC.InitAbilityActorInfo(monobehaviour, gameObject);

        InitializeAbilities();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _networkAttributeDatas.OnListChanged += OnChangedNetworkList;

        InitializeAttributes();
    }

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();

        InitializeEffects();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        _networkAttributeDatas.OnListChanged -= OnChangedNetworkList;

        ReleaseAttributes();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        ASC.Dispose();
    }

    protected override void Awake()
    {
        base.Awake();

        _networkAttributeDatas.Initialize(this);
    }

    private void OnChangedAttributeCurrentValue(string attributeName, float prev, float next)
    {
        if (IsServer == false)
            return;

        SetAttributeValue(attributeName, ASC.GetAttribute(attributeName));

        if (attributeName != SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Speed)
            return;

        MoveSpeed = next;
    }

    private void InitializeAttributes()
    {
        if (_initializeAttributes.IsNullOrEmpty())
            return;

        var length = _initializeAttributes.Count;
        for (var i = 0; i < length; ++i)
        {
            var attributeName = _initializeAttributes[i];
            if (_changeCallbackContainer.ContainsKey(attributeName) == false)
                _changeCallbackContainer[attributeName] = (prev, next) => { OnChangedAttributeCurrentValue(attributeName, prev, next); };

            var attribute = ASC.GetAttribute(attributeName);
            attribute.OnCurrentValueChanged += _changeCallbackContainer[attributeName];
        }
    }

    private void ReleaseAttributes()
    {
        if (_initializeAttributes.IsNullOrEmpty())
            return;

        var length = _initializeAttributes.Count;
        for (var i = 0; i < length; ++i)
        {
            var attributeName = _initializeAttributes[i];
            if (_changeCallbackContainer.ContainsKey(attributeName) == false)
                continue;

            var attribute = ASC.GetAttribute(attributeName);
            attribute.OnCurrentValueChanged -= _changeCallbackContainer[attributeName];
        }
    }

    private void InitializeEffects()
    {
        if (_initializeEffects.IsNullOrEmpty())
            return;

        var length = _initializeEffects.Count;
        for (var i = 0; i < length; ++i)
        {
            ApplyGameplayEffect(_initializeEffects[i]);
        }
    }

    private void InitializeAbilities()
    {
        if (_initializeAbilities.IsNullOrEmpty())
            return;

        var length = _initializeAbilities.Count;
        for (int i = 0; i < length; ++i)
        {
            ASC.GrantAbility(_initializeAbilities[i].CreateAbility());
        }
    }

    private void OnChangedNetworkList(NetworkListEvent<NetworkAttributeData> changeEvent)
    {
        OnChangedNetworkAttributeList?.Invoke(changeEvent, _networkAttributeDatas);
    }
}
