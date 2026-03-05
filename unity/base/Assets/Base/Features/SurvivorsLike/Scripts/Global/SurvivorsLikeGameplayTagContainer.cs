using CycloneGames.GameplayTags.Runtime;
using UnityEngine;

[assembly: RegisterGameplayTagsFrom(typeof(SurvivorsLikeGameplayTagContainer))]

public static class SurvivorsLikeGameplayTagContainer
{
    //  Attributes
    public const string SurvivorsLike_Attribute_MaxHealth = "SurvivorsLike.Attribute.MaxHealth";
    public const string SurvivorsLike_Attribute_Health = "SurvivorsLike.Attribute.Health";
    public const string SurvivorsLike_Attribute_Speed = "SurvivorsLike.Attribute.Speed";
    public const string SurvivorsLike_Attribute_Damage = "SurvivorsLike.Attribute.Damage";
    public const string SurvivorsLike_Attribute_Pickup_Range = "SurvivorsLike.Attribute.Pickup.Range";
    public const string SurvivorsLike_Attribute_Pickup_Bonus = "SurvivorsLike.Attribute.Pickup.Bonus";
    public const string SurvivorsLike_Attribute_Ability_Count = "SurvivorsLike.Attribute.Ability.Count";
    public const string SurvivorsLike_Attribute_Ability_Speed = "SurvivorsLike.Attribute.Ability.Speed";
    public const string SurvivorsLike_Attribute_Find_Range = "SurvivorsLike.Attribute.Find.Range";
    public const string SurvivorsLike_Attribute_Level = "SurvivorsLike.Attribute.Level";

    //  Abilities
    public const string SurvivorsLike_Ability_Arrow = "SurvivorsLike.Ability.Arrow";

    //  Tag
    public const string SurvivorsLike_Cooldown_Ability_Arrow = "SurvivorsLike.Cooldown.Ability.Arrow";
}
