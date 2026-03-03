using UnityEngine;

public class IKCharacter : Pawn
{
    [SerializeField] private float _moveSpeed;
    private IKFootSolver[] _iKFootSolvers = null;
    private IKHead _iKHead = null;

    public void SetStepSpeed(float stepSpeed)
    {
        var length = _iKFootSolvers.Length;
        for (var i = 0; i < length; ++i)
            _iKFootSolvers[i].SetStepDistance(stepSpeed);
    }

    public void SetStepDistance(float stepDistance)
    {
        var length = _iKFootSolvers.Length;
        for (var i = 0; i < length; ++i)
            _iKFootSolvers[i].SetStepDistance(stepDistance);
    }

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        var dir = InputService.MoveDirection;
        transform.position += dir * Time.deltaTime * _moveSpeed;
        _iKHead.LookAt(dir);
    }

    protected override void Awake()
    {
        base.Awake();

        _iKFootSolvers = GetComponentsInChildren<IKFootSolver>();
        _iKHead = GetComponentInChildren<IKHead>();
    }
}
