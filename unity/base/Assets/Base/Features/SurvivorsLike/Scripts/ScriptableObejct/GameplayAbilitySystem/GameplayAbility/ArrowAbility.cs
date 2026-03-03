using CycloneGames.GameplayAbilities.Runtime;
using UnityEngine;

public class ArrowAbility : GameplayAbility
{
    public override GameplayAbility CreatePoolableInstance()
    {
        var ability = new ArrowAbility();
        ability.Initialize(
            this.Name,
            this.InstancingPolicy,
            this.NetExecutionPolicy,
            this.CostEffectDefinition,
            this.CooldownEffectDefinition,
            this.AbilityTags,
            this.ActivationBlockedTags,
            this.ActivationRequiredTags,
            this.CancelAbilitiesWithTag,
            this.BlockAbilitiesWithTag
        );

        return ability;
    }
}
