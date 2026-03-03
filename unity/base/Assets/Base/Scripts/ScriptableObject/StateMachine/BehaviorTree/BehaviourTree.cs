using UnityEngine;

public sealed class BehaviourTree
{
    private Leaf _root = null;

    public BehaviourTree(Leaf root)
    {
        _root = root;
    }

    public async void Update(MonoBehaviour behaviour)
    {
        await _root.Execute(behaviour);
    }
}
