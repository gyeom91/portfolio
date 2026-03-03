using UnityEngine;

public class Pawn : Actor, IWorldActor
{
    public Vector3 Position => transform.position;
    public Vector3Int GridPosition { get; private set; }

    public void Behaviour()
    {
        OnBehaviour();
    }

    public void FixedBehaviour()
    {
        OnFixedBehaviour();
    }

    public void SetGridPosition(Vector3Int position)
    {
        GridPosition = position;
    }

    protected virtual void OnBehaviour()
    {

    }

    protected virtual void OnFixedBehaviour()
    {

    }
}
