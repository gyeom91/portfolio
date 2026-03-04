using System.Collections.Generic;
using UnityEngine;

public class WorldService : Service
{
    public enum ERange
    {
        Box,
        Circle,
    }

    [SerializeField] protected Vector2Int _worldSize;
    [SerializeField] protected GameObject _groundPrefab;
    [SerializeField] protected GameObject _wallPrefab;
    protected Grid _grid = null;
    protected Dictionary<Vector3Int, Cell> _cells = new();

    public void UpdatePosition(IWorldActor worldActor)
    {
        Vector3Int newKey = _grid.WorldToCell(worldActor.Position);
        Vector3Int oldKey = worldActor.GridPosition;

        if (oldKey != newKey)
        {
            if (_cells.TryGetValue(oldKey, out Cell oldCell))
            {
                oldCell.Remove(worldActor);
            }

            if (_cells.TryGetValue(newKey, out Cell newCell))
            {
                newCell.Add(worldActor);
                worldActor.SetGridPosition(newKey);
            }
        }
    }

    public void GetPositionToCellRange(Vector3 position, ERange eRange, float range, List<Cell> output)
    {
        var centerKey = _grid.WorldToCell(position);
        var cellRange = Mathf.CeilToInt(range);
        var sqrRange = range * range;

        for (int x = -cellRange; x <= cellRange; x++)
        {
            for (int y = -cellRange; y <= cellRange; y++)
            {
                Vector3Int targetKey = centerKey + new Vector3Int(x, y, 0);

                if (eRange == ERange.Circle)
                {
                    Vector3 targetWorldPos = _grid.GetCellCenterWorld(targetKey);
                    if ((position - targetWorldPos).sqrMagnitude > sqrRange)
                        continue;
                }

                if (_cells.TryGetValue(targetKey, out Cell cell))
                {
                    output.Add(cell);
                }
            }
        }
    }

    public Cell GetPositionToCell(Vector3 position)
    {
        var key = _grid.WorldToCell(position);
        if (_cells.TryGetValue(key, out Cell cell) == false)
            return null;

        return _cells[key];
    }

    protected override void Awake()
    {
        base.Awake();

        _grid = GetComponent<Grid>();
    }

    protected virtual void Start()
    {
        var sceneController = SceneController.Instance;
        var poolService = sceneController.GetService<PoolService>();

        int startX = -_worldSize.x / 2;
        int endX = startX + _worldSize.x - 1;
        int startY = -_worldSize.y / 2;
        int endY = startY + _worldSize.y - 1;
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                var cellPosition = new Vector3Int(x, y, 0);
                var worldPosition = _grid.GetCellCenterWorld(cellPosition);

                IWorldActor worldActor = null;
                if (x == startX || x == endX || y == startY || y == endY)
                    worldActor = poolService.Spawn<IWorldActor>(_wallPrefab, worldPosition, Quaternion.identity, transform);
                else
                    worldActor = poolService.Spawn<IWorldActor>(_groundPrefab, worldPosition, Quaternion.identity, transform);

                if (worldActor.IsNull())
                    continue;

                worldActor.SetGridPosition(cellPosition);

                var cell = new Cell();
                cell.Add(worldActor);

                _cells.Add(cellPosition, cell);
            }
        }

        StaticBatchingUtility.Combine(gameObject);
    }
}
