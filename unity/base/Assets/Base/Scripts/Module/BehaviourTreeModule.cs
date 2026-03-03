using UnityEngine;

public class BehaviourTreeModule : BaseMonobehaviour
{
    [SerializeField] private Leaf _root;

    protected virtual async void Start()
    {
        if (_root.IsNull())
            return;

        await _root.Execute();
    }

    protected virtual async void Update()
    {
        if (_root.IsNull())
            return;

        await _root.Execute(this);
    }

    protected virtual async void FixedUpdate()
    {
        if (_root.IsNull())
            return;

        await _root.FixedExecute(this);
    }
}
