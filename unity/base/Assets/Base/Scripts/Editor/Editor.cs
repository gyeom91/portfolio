using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class Editor
{
    [MenuItem("Tools/Play Default Scene %Q")]
    public static void PlayDefaultScene()
    {
        var scenePath = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

        EditorSceneManager.playModeStartScene = sceneAsset;
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    [MenuItem("Tools/Play Current Scene %E")]
    public static void PlayCurrentScene()
    {
        EditorSceneManager.playModeStartScene = null;
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Clear Cache")]
    public static void ClearCache()
    {
        Caching.ClearCache();
    }
}
