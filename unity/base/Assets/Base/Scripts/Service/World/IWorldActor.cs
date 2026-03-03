using UnityEngine;

public interface IWorldActor
{
    Vector3 Position { get; }
    Vector3Int GridPosition { get; }

    void SetGridPosition(Vector3Int position);
}
