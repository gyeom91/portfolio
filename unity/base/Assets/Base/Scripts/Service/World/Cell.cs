using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private HashSet<IWorldActor> _actors = new();

    public void Add(IWorldActor worldActor)
    {
        if (_actors.Contains(worldActor))
            return;

        _actors.Add(worldActor);
    }

    public void Remove(IWorldActor worldActor)
    {
        if (_actors.Contains(worldActor) == false)
            return;

        _actors.Remove(worldActor);
    }
}
