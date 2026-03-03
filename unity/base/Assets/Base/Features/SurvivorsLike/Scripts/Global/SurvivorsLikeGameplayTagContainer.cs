using CycloneGames.GameplayTags.Runtime;
using UnityEngine;

[assembly: RegisterGameplayTagsFrom(typeof(SurvivorsLikeGameplayTagContainer))]

public class SurvivorsLikeGameplayTagContainer
{
    public const string Attribute_Skill_Count = "Attribute.Skill.Count";

    public const string Skill_Arrow_Cooldown = "Cooldown.Skill.Arrow";
}
