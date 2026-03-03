using UnityEngine;

public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    public static T Instance
    {
        get
        {
            if (_instance.IsNull())
            {
                var assets = Resources.LoadAll<T>("");
                if (assets.IsNull() || assets.Length == 0)
                    throw new System.Exception();
                else if (assets.Length > 1)
                    throw new System.Exception();

                _instance = assets[0];
            }

            return _instance;
        }
    }

    private static T _instance;
}
