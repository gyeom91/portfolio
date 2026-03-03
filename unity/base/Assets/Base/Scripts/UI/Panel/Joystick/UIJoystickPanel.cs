using System;
using UnityEngine;

public class UIJoystickPanel : UIPanel
{
    [SerializeField] private UIVirtualJoystick _moveJoystick;
    [SerializeField] private UIVirtualJoystick _lookAtJoystick;

    private void Start()
    {
        var sceneController = SceneController.Instance;
        var inputService = sceneController.GetService<InputService>();
        if (inputService.IsNull())
            return;

        _moveJoystick.OnChangedValue -= inputService.SetMoveDirection;
        _moveJoystick.OnChangedValue += inputService.SetMoveDirection;
        _lookAtJoystick.OnChangedValue -= inputService.SetLookAtDirection;
        _lookAtJoystick.OnChangedValue += inputService.SetLookAtDirection;
    }
}
