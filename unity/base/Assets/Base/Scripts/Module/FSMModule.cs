using System;
using UnityEngine;

public class FSMModule : BaseMonobehaviour
{
    protected FSMState _currentState { get; private set; }

    [SerializeField] private FSMNode _root;
    [SerializeField] private FSMNode[] _nodes;
    private static event Action<string> _onTrigger;

    public static async Awaitable Trigger(string trigger)
    {
        _onTrigger?.Invoke(trigger);
    }

    protected virtual async Awaitable Enter()
    {
        _currentState = null;
    }

    protected virtual async Awaitable Stay()
    {
        if (_currentState.IsNull())
            return;

        await _currentState.Stay();
    }

    protected virtual async Awaitable Exit()
    {
        _currentState = null;
    }

    protected override void Awake()
    {
        base.Awake();

        _onTrigger += OnTrigger;
    }

    private async void Start()
    {
        await Enter();

        if (_root.IsNull())
            return;

        await _root.Enter();
    }

    private async void Update()
    {
        await Stay();
    }

    private async void OnDestroy()
    {
        _onTrigger -= OnTrigger;

        await Exit();
    }

    private async void OnTrigger(string trigger)
    {
        var nodeLength = _nodes.Length;
        for (var j = 0; j < nodeLength; ++j)
        {
            var node = _nodes[j];
            if (node is FSMState state == false)
                continue;

            if (state.HasTrigger(trigger) == false)
                continue;

            await ChangeState(state);
            break;
        }
    }

    private async Awaitable ChangeState(FSMState changeState)
    {
        if (_currentState == changeState)
        {
            await _currentState.Trigger();
            return;
        }

        if (_currentState.IsNull() == false)
            await _currentState.Exit();

        _currentState = Array.Find(_nodes, node =>
        {
            if (node is FSMState state == false)
                return false;

            return state == changeState;
        }) as FSMState;

        if (_currentState.IsNull() == false)
            await _currentState.Enter();
    }
}
