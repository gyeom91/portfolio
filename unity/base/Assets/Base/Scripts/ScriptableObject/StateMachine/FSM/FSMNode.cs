using UnityEngine;

public abstract class FSMNode : Node
{
    protected const string PATH = Constants.FSM_PATH;

    public virtual async Awaitable Trigger()
    {

    }

    public virtual async Awaitable Enter()
    {

    }

    public virtual async Awaitable Stay()
    {

    }

    public virtual async Awaitable FixedStay()
    {

    }

    public virtual async Awaitable Exit()
    {

    }

    public override async Awaitable Execute()
    {

    }
}
