using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = PATH + "Trigger")]
public class Trigger : Decorator
{
    [SerializeField] private string _trigger;
    private static Queue<string> _tiggers = new();

    public static void Enqueue(string triggerName)
    {
        _tiggers.Enqueue(triggerName);
    }

    public override async Awaitable<EState> Execute(MonoBehaviour behaviour)
    {
        if (_tiggers.Peek() == _trigger)
        {
            _tiggers.Dequeue();

            return await _child.Execute(behaviour);
        }

        return EState.Succeed;
    }
}
