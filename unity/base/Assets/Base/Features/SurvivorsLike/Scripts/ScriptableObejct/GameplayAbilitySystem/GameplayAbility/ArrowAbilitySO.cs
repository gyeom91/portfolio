using CycloneGames.GameplayAbilities.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "GA_Arrow", menuName = Constants.GAMEPLAY_ABILITY_SYSTEM_PATH + "GA_Arrow")]
public class ArrowAbilitySO : GameplayAbilitySO
{
    [SerializeField] private string _arrowPrefabName;
    [SerializeField] private Vector3 _positionOffset;

    public override GameplayAbility CreateAbility()
    {
        var ability = new ArrowAbility(_arrowPrefabName, _positionOffset);
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
