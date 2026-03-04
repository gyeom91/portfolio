using System.Collections.Generic;
using UnityEngine;

public class Monster : BattleCharacter
{
    protected Transform _target { get; private set; }

    private List<Cell> _cells = new();

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        _target = null;
        _cells.Clear();

        var attribute = ASC.GetAttribute(SurvivorsLikeGameplayTagContainer.SurvivorsLike_Attribute_Find_Range);
        var findRange = attribute.CurrentValue;
        var minSqrDistance = findRange * findRange;
        Transform closestHero = null;

        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        battleWorldService.GetPositionToCellRange(Position, WorldService.ERange.Circle, findRange, _cells);

        foreach (var cell in _cells)
        {
            foreach (var actor in cell.Actors)
            {
                if (actor is Hero hero)
                {
                    float sqrDistance = (transform.position - hero.transform.position).sqrMagnitude;

                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        closestHero = hero.transform;
                    }
                }
            }
        }

        _target = closestHero;
        if (_target.IsNull())
            return;

        _moveDirection = (_target.position - transform.position).normalized;
    }
}
