using UnityEngine;

[CreateAssetMenu(fileName = "Repeat", menuName = PATH + "Repeat")]
public class Repeat : Decorator
{
    [SerializeField] private int _count;

    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        for (var i = 0; i < _count; ++i)
        {
            var eState = await _child.Execute(behaviour);
            switch (eState)
            {
                case EState.Succeed:
                    {
                        continue;
                    }

                case EState.Failed:
                    {
                        return EState.Failed;
                    }

                case EState.Running:
                    {
                        --i;

                        break;
                    }
            }
        }

        return EState.Succeed;
    }
}
