using Unity.Cinemachine;
using UnityEngine;

public class CinemachineHandler : BaseMonobehaviour
{
    protected CinemachineCamera _cinemachineCamera { get; private set; }

    public virtual void SetFollowTarget(Transform target)
    {
        _cinemachineCamera.Follow = target;
    }

    public virtual void SetLookAtTarget(Transform target)
    {
        _cinemachineCamera.LookAt = target;
    }

    public void SetPriority(int priority)
    {
        _cinemachineCamera.Priority.Value = priority;

        if (priority == 0)
            OnDeactive();
        else
            OnActive();
    }

    protected override void Awake()
    {
        base.Awake();

        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineCamera.Priority.Enabled = true;
        _cinemachineCamera.Priority.Value = 0;
    }

    protected virtual void OnActive()
    {

    }

    protected virtual void OnDeactive()
    {

    }
}
