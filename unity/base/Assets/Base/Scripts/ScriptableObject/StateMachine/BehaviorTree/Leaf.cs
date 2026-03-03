using UnityEngine;

public abstract class Leaf : Node
{
    public enum EState
    {
        Succeed,
        Failed,
        Running,
    }

    protected const string PATH = Constants.BEHAVIOUR_TREE_PATH;

    public override async Awaitable Execute()
    {

    }

    public virtual async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        return EState.Succeed;
    }

    public virtual async Awaitable<EState> FixedExecute(MonoBehaviour behaviour)
    {
        return EState.Succeed;
    }
}
