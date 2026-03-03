using UnityEngine;

public class Monster : BattleCharacter
{
    protected Transform _target { get; private set; }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        _target = null;

        if (_networkObject.IsOwnedByServer == false)
            return;

        var sceneController = SceneController.Instance;
        var battleWorldService = sceneController.GetService<BattleWorldService>();
        var minDistance = float.MaxValue;
        battleWorldService.ForeachActive(pawn =>
        {
            if (pawn is Hero hero == false)
                return;

            var heroTransform = hero.transform;
            var distance = Vector3.Distance(transform.position, heroTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _target = hero.transform;
            }
        });
        if (_target.IsNull())
            return;

        _moveDirection = (_target.position - transform.position).normalized;
    }
}
