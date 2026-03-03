using CycloneGames.GameplayAbilities.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "GA_Arrow", menuName = Constants.GAMEPLAY_ABILITY_SYSTEM_PATH + "GA_Arrow")]
public class ArrowAbilitySO : GameplayAbilitySO
{
    public override GameplayAbility CreateAbility()
    {
        var ability = new ArrowAbility();
        ability.Initialize(
            AbilityName,
            InstancingPolicy,
            NetExecutionPolicy,
            CostEffect?.GetGameplayEffect(),
            CooldownEffect?.GetGameplayEffect(),
            AbilityTags,
            ActivationBlockedTags,
            ActivationRequiredTags,
            CancelAbilitiesWithTag,
            BlockAbilitiesWithTag
        );

        return ability;
    }
}
