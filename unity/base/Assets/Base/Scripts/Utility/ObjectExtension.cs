using UnityEngine;

public static class ObjectExtension
{
    public static bool IsNull(this object obj)
    {
        if (ReferenceEquals(obj, null))
            return true;

        if (obj is UnityEngine.Object unity)
            return unity == null;

        return false;
    }

    public static void Exit(this object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
