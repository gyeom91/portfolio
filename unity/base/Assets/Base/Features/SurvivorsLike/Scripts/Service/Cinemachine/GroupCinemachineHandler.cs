using Unity.Cinemachine;
using UnityEngine;

public class GroupCinemachineHandler : CinemachineHandler
{
    [SerializeField] private CinemachineTargetGroup _targetGroupPrefab;
    private CinemachineTargetGroup _targetGroup = null;

    public void AddTarget(Transform target)
    {
        if (_targetGroup.IsNull())
            return;

        _targetGroup.AddMember(target, 1, 1);
    }

    public void RemoveTarget(Transform target)
    {
        if (_targetGroup.IsNull())
            return;

        _targetGroup.RemoveMember(target);
    }

    public override void SetFollowTarget(Transform target)
    {
        base.SetFollowTarget(_targetGroup.transform);
    }

    public override void SetLookAtTarget(Transform target)
    {
        base.SetLookAtTarget(_targetGroup.transform);
    }

    protected override void Awake()
    {
        base.Awake();

        _targetGroup = Instantiate(_targetGroupPrefab, transform.parent);
    }

    protected override void OnActive()
    {
        base.OnActive();
    }
}
