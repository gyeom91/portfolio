using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadAction", menuName = PATH + "SceneLoadAction")]
public class SceneLoadAction : FSMAction
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private LoadSceneMode _loadSceneMode;

    public override async Awaitable Enter()
    {
        try
        {
            this.LoadScene(_sceneIndex, _loadSceneMode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
