using UnityEngine;

public class Environment : BaseMonobehaviour, IWorldActor
{
    public Vector3 Position => transform.position;
    public Vector3Int GridPosition { get; private set; }

    public void SetGridPosition(Vector3Int position)
    {
        GridPosition = position;
    }
}
