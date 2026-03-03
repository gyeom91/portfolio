using UnityEngine;

[CreateAssetMenu(fileName = "RepeatUntilFail", menuName = PATH + "RepeatUntilFail")]
public class RepeatUntilFail : Decorator
{
    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        do
        {
            var eState = await _child.Execute(behaviour);
            if (eState == EState.Failed)
                break;
        }
        while (true);

        return EState.Succeed;
    }
}
