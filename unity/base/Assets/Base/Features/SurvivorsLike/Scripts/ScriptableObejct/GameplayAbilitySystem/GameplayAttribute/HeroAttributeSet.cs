using CycloneGames.GameplayAbilities.Runtime;
using UnityEngine;

public class HeroAttributeSet : AttributeSet
{
    public GameplayAttribute PickupRange { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Pickup_Range);
    public GameplayAttribute PickupBonus { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Pickup_Bonus);
    public GameplayAttribute CreateCount { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Ability_Create_Count);
    public GameplayAttribute Level { get; } = new GameplayAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Level);

    public HeroAttributeSet(string characterName)
    {
        var heroTable = TableManager.Instance.GetTable<HeroTable>();
        if (heroTable.TryGetData(characterName, out var heroData) == false)
            return;

        Level.SetBaseValue(heroData.Level);
        Level.SetCurrentValue(heroData.Level);
    }
}
