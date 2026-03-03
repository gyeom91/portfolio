using UnityEngine;

public static class ComponentExtension
{
    public static bool TryGetComponents<T>(this Component component, out T[] output)
    {
        output = component.GetComponentsInChildren<T>();
        return output.IsNull() == false;
    }

    public static LayerMask LayerMask(this Component component)
    {
        var gameObject = component.gameObject;
        return gameObject.layer;
    }
}
