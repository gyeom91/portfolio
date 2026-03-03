using CycloneGames.GameplayTags.Runtime;
using UnityEngine;

[assembly: RegisterGameplayTagsFrom(typeof(SurvivorsLikeGameplayTagContainer))]
public class SurvivorsLikeGameplayTagContainer
{
    public const string Skill_Arrow_Cooldown = "Cooldown.Skill.Arrow";
}
