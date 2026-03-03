using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = PATH + "Sequence")]
public class Sequence : Composite
{
    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        var length = _childs.Length;
        for (var i = 0; i < length; ++i)
        {
            var child = _childs[i];
            var eState = await child.Execute(behaviour);
            if (eState != EState.Running)
                break;
        }

        return EState.Succeed;
    }
}
