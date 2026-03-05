using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class CharacterAttributeSet : AttributeSet
{
    public GameplayAttribute MaxHealth { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_MaxHealth);
    public GameplayAttribute Health { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Health);
    public GameplayAttribute Damage { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Damage);
    public GameplayAttribute Speed { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Speed);

    public override void PreAttributeChange(GameplayAttribute attribute, ref float newValue)
    {
        base.PreAttributeChange(attribute, ref newValue);

        if (attribute == Health)
        {
            newValue = System.Math.Clamp(newValue, 0, GetCurrentValue(MaxHealth));
        }
    }

    public override void PostGameplayEffectExecute(GameplayEffectModCallbackData data)
    {
        base.PostGameplayEffectExecute(data);

        var attribute = GetAttribute(data.Modifier.AttributeName);
        if (attribute == null) return;

        if (attribute == Health)
        {
            SetBaseValue(Health, System.Math.Clamp(GetBaseValue(Health), 0, GetCurrentValue(MaxHealth)));

            if (Health.BaseValue == 0)
            {
                var asc = this.OwningAbilitySystemComponent;
                var gameObject = asc.AvatarActor as GameObject;
                if (gameObject.TryGetComponent<NetworkObject>(out var networkObject) == false)
                    return;

                networkObject.Despawn();
            }
        }
    }
}
