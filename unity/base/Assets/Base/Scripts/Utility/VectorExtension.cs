using UnityEngine;

public static class VectorExtension
{
    public static Vector3 ToVector(this Vector3Int v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vector2 ToVector(this Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 Y2Z(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
}
