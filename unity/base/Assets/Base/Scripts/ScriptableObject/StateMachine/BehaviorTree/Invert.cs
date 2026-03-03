using UnityEngine;

[CreateAssetMenu(fileName = "Invert", menuName = PATH + "Invert")]
public class Invert : Decorator
{
    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        var state = await _child.Execute(behaviour);
        if (state == EState.Succeed)
        {
            return EState.Failed;
        }
        else if (state == EState.Failed)
        {
            return EState.Succeed;
        }

        return EState.Running;
    }
}
