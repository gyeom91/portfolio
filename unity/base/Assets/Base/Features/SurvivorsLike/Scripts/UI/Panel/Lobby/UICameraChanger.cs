using System;
using UnityEngine;

public class UICameraChanger : BaseMonobehaviour
{
    [Serializable]
    private enum ECamera
    {
        Group,
        Solo,
    }

    [SerializeField] private ECamera _eCamera;
    private UI _targetUI = null;

    protected override void Awake()
    {
        base.Awake();

        _targetUI = GetComponent<UI>();
        _targetUI.OnActivate += OnActivateTargetUI;
    }

    private void OnActivateTargetUI(bool active)
    {
        if (active == false)
            return;

        var sceneController = SceneController.Instance as LobbySceneController;
        var cinemachineService = sceneController.GetService<CinemachineService>();
        switch (_eCamera)
        {
            case ECamera.Group:
                cinemachineService.ChangeHandler<GroupCinemachineHandler>();
                break;
            case ECamera.Solo:
                cinemachineService.ChangeHandler<LobbyCinemachineHandler>();
                break;
        }
    }
}
