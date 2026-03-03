using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleWorldService : WorldService
{
    public IReadOnlyCollection<Pawn> Pawns { get => _pawns; }

    [SerializeField] private float _yOffset;
    private LinkedList<Pawn> _pawns = new();

    public void ForeachActive(Action<Pawn> callback)
    {
        var node = _pawns.First;
        while (node.IsNull() == false)
        {
            var nextNode = node.Next;
            var pawn = node.Value;
            if (pawn.IsActive)
                callback.Invoke(pawn);

            node = nextNode;
        }
    }

    public Vector3 GetRandomPosition()
    {
        var halfX = _worldSize.x / 2;
        var randomX = UnityEngine.Random.Range(-halfX, halfX);
        var halfZ = _worldSize.y / 2;
        var randomZ = UnityEngine.Random.Range(-halfZ, halfZ);
        return new Vector3(randomX, _yOffset, randomZ);
    }

    public virtual void AddWorldPawn(Pawn pawn)
    {
        if (_pawns.Contains(pawn))
            return;

        _pawns.AddLast(pawn);
    }

    public virtual void RemoveWorldPawn(Pawn pawn)
    {
        if (_pawns.Contains(pawn) == false)
            return;

        _pawns.Remove(pawn);
    }

    protected virtual void Update()
    {
        var sceneController = SceneController.Instance as BattleSceneController;
        if (sceneController.IsNull() || sceneController.Freeze)
            return;

        ForeachActive(pawn =>
        {
            if (pawn is Hero)
                return;

            pawn.Behaviour();

            UpdatePosition(pawn);
        });
    }

    protected virtual void FixedUpdate()
    {
        var sceneController = SceneController.Instance as BattleSceneController;
        if (sceneController.IsNull() || sceneController.Freeze)
            return;

        ForeachActive(pawn =>
        {
            if (pawn is Hero)
                return;

            pawn.FixedBehaviour();

            UpdatePosition(pawn);
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _pawns.Clear();
        _pawns = null;
    }
}
