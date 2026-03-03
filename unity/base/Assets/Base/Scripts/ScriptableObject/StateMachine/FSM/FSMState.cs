using UnityEngine;

[CreateAssetMenu(fileName = "FSMState", menuName = PATH + "FSMState")]
public class FSMState : FSMNode
{
    [SerializeField] protected string _trigger;
    [SerializeField] protected FSMAction[] _actions;

    public bool HasTrigger(string trigger)
    {
        return _trigger == trigger;
    }

    public override async Awaitable Trigger()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
            await _actions[i].Trigger();
    }

    public override async Awaitable Enter()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
            await _actions[i].Enter();
    }

    public override async Awaitable Stay()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
            await _actions[i].Stay();
    }

    public override async Awaitable Exit()
    {
        var length = _actions.Length;
        for (var i = 0; i < length; ++i)
            await _actions[i].Exit();
    }
}
