using UnityEngine;

public class InputService : Service
{
    public static Vector3 MoveDirection { get; private set; }
    public static Vector3 LookAtDirection { get; private set; }

    private UnityInput _unityInput = null;

    public void SetMoveDirection(Vector2 direction)
    {
        MoveDirection = direction.Y2Z();
    }

    public void SetLookAtDirection(Vector2 direction)
    {
        LookAtDirection = direction.Y2Z();
    }

    protected override void Awake()
    {
        base.Awake();

        _unityInput = new UnityInput();
        _unityInput.Player.Move.performed += MovePerformed;
        _unityInput.Player.Move.canceled += MoveCanceled;
        _unityInput.Player.Look.performed += LookPerformed;
        _unityInput.Player.Look.canceled += LookCanceled;
    }

    protected virtual void OnEnable()
    {
        _unityInput.Enable();
    }

    protected virtual void OnDisable()
    {
        _unityInput.Disable();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _unityInput.Disable();
        _unityInput.Dispose();
        _unityInput = null;
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveDirection = obj.ReadValue<Vector2>().Y2Z();
    }

    private void MoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveDirection = obj.ReadValue<Vector2>().Y2Z();
    }

    private void LookCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        LookAtDirection = obj.ReadValue<Vector2>().Y2Z();
    }

    private void LookPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        LookAtDirection = obj.ReadValue<Vector2>().Y2Z();
    }
}
