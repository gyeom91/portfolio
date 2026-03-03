using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> values)
    {
        return values.IsNull() || values.Count == 0;
    }

    public static T GetRandomValue<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        if (list.IsNull() || list.Count == 0)
            return;

        var count = list.Count;
        while (count > 1)
        {
            --count;
            var next = UnityEngine.Random.Range(0, count + 1);

            T temp = list[next];
            list[next] = list[count];
            list[count] = temp;
        }
    }
}
