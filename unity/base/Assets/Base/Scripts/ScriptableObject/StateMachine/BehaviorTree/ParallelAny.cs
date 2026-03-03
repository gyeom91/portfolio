using UnityEngine;

[CreateAssetMenu(fileName = "ParallelAny", menuName = PATH + "ParallelAny")]
public class ParallelAny : Composite
{
    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        var length = _childs.Length;
        for (var i = 0; i < length; ++i)
        {
            var child = _childs[i];
            var state = await child.Execute(behaviour);
            if (state != EState.Running)
                return state;
        }

        return EState.Running;
    }
}
