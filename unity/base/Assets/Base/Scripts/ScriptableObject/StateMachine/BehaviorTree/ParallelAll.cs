using UnityEngine;

[CreateAssetMenu(fileName = "ParallelAll", menuName = PATH + "ParallelAll")]
public class ParallelAll : Composite
{
    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        var succeeded = 0;
        var length = _childs.Length;
        for (var i = 0; i < length; ++i)
        {
            var child = _childs[i];
            var eState = await child.Execute(behaviour);
            switch (eState)
            {
                case EState.Succeed:
                    {
                        ++succeeded;
                        break;
                    }

                case EState.Failed:
                    {
                        return EState.Failed;
                    }

                case EState.Running:
                    {
                        break;
                    }
            }
        }

        if (length == succeeded)
            return EState.Succeed;
        else
            return EState.Running;
    }
}
