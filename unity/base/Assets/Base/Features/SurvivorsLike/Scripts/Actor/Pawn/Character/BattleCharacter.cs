using System;
using System.Collections.Generic;
using CycloneGames.GameplayTags.Runtime;
using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkSpawnable))]
public class BattleCharacter : Character, INetworkInitialize, INetworkRelease
{
    public AbilitySystemComponent ASC { get; private set; }

    protected override float _posSpeed => _moveSpeed;
    protected NetworkObject _networkObject { get; private set; }
    protected Dictionary<string, Action<float, float>> _changeCallbackContainer { get; private set; } = new();

    [SerializeField, AttributeNameSelector] private List<string> _initializeAttributes;
    [SerializeField] private List<GameplayEffectSO> _initializeEffects;
    [SerializeField] private List<GameplayAbilitySO> _initializeAbilities;
    private float _moveSpeed = 0;

    public void TryActivateAbilityWithTag(GameplayTag gameplayTag)
    {
        if (_networkObject.IsServer() == false)
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
        if (_networkObject.IsServer() == false)
            return;

        var ge = gameplayEffectSO.GetGameplayEffect();
        var spec = GameplayEffectSpec.Create(ge, ASC);
        ASC.ApplyGameplayEffectSpecToSelf(spec);
    }

    public void OnPrevInitialize(NetworkManager networkManager)
    {
        if (networkManager.IsServer)
            return;

        var sceneController = SceneController.Instance;
        var networkService = sceneController.GetService<NetworkService>();
        networkService.RegisterEvents(Constants.EHeader.ATTRIBUTE_CHANGE_VALUE, OnReceiveChangedAttributeValue);
    }

    public virtual void OnInitialize(NetworkObject networkObject)
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.AddWorldPawn(this);

        InitializeAttributes();
    }

    public void OnPostInitialize()
    {
        if (_networkObject.IsServer() == false)
            return;

        InitializeEffects();
    }

    public void OnPrevRelease()
    {
        if (_networkObject.IsServer())
            return;

        var sceneController = SceneController.Instance;
        var networkService = sceneController.GetService<NetworkService>();
        networkService.UnregisterEvents(Constants.EHeader.ATTRIBUTE_CHANGE_VALUE, OnReceiveChangedAttributeValue);
    }

    public virtual void OnRelease()
    {
        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.RemoveWorldPawn(this);

        ReleaseAttributes();
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        ASC.Tick(Time.deltaTime, _networkObject.IsServer());
    }

    protected override void Awake()
    {
        base.Awake();

        _networkObject = GetComponent<NetworkObject>();

        var effectContextFactory = new GameplayEffectContextFactory();
        ASC = new AbilitySystemComponent(effectContextFactory);
        ASC.InitAbilityActorInfo(this, gameObject);
        ASC.AddAttributeSet(new CharacterAttributeSet());

        InitializeAbilities();
    }

    private void OnDestroy()
    {
        ASC.Dispose();
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

    private void OnChangedAttributeCurrentValue(string attributeName, float prev, float next)
    {
        if (_networkObject.IsServer() == false)
            return;

        var sceneController = SceneController.Instance;
        var networkService = sceneController.GetService<NetworkService>();
        var message = new ChangeAttributeMessage();
        message.EHeader = Constants.EHeader.ATTRIBUTE_CHANGE_VALUE;
        message.AttributeName = attributeName;
        message.CurrentValue = next;
        message.NetworkObjectID = _networkObject.NetworkObjectId;

        var attribute = ASC.GetAttribute(attributeName);
        message.BaseValue = attribute.BaseValue;

        networkService.SendToAllClient(message, NetworkDelivery.ReliableSequenced);

        if (attributeName != SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Speed)
            return;

        _moveSpeed = next;
    }

    private void OnReceiveChangedAttributeValue(Constants.EHeader eHeader, FastBufferReader reader)
    {
        if (reader.TryGetValue<ChangeAttributeMessage>(out var message) == false)
            return;

        var networkManager = NetworkManager.Singleton;
        var networkObject = networkManager.GetNetworkObject(message.NetworkObjectID);
        if (networkObject.TryGetComponent<BattleCharacter>(out var character) == false)
            return;

        var asc = character.ASC;
        var attribute = asc.GetAttribute(message.AttributeName.ToString());
        attribute.SetBaseValue(message.BaseValue);
        attribute.SetCurrentValue(message.CurrentValue);
    }
}
