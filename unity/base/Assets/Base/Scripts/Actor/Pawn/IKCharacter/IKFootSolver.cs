using UnityEngine;

public class IKFootSolver : BaseMonobehaviour
{
    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private Transform _body = default;
    [SerializeField] private IKFootSolver[] _otherIKFootSolvers = default;
    [SerializeField] private float _stepSpeed = 1;
    [SerializeField] private float _stepDistance = 4;
    [SerializeField] private float _stepLength = 4;
    [SerializeField] private float _stepHeight = 1;
    [SerializeField] private Vector3 _footOffset = default;
    private float _footSpacing;
    private Vector3 _oldPosition;
    private Vector3 _currentPosition;
    private Vector3 _newPosition;
    private Vector3 _oldNormal;
    private Vector3 _currentNormal;
    private Vector3 _newNormal;
    private float _lerp;

    public bool IsMoving()
    {
        return _lerp < 1;
    }

    public void SetStepSpeed(float stepSpeed)
    {
        _stepSpeed = stepSpeed;
    }

    public void SetStepDistance(float stepDistance)
    {
        _stepDistance = stepDistance;
    }

    public void SetStepLength(float stepLength)
    {
        _stepLength = stepLength;
    }

    protected override void Awake()
    {
        base.Awake();

        _footSpacing = transform.localPosition.x;
        _currentPosition = _newPosition = _oldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    protected virtual void Update()
    {
        transform.position = _currentPosition;
        transform.up = _currentNormal;

        Ray ray = new Ray(_body.position + (_body.right * _footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
        {
            if (Vector3.Distance(_newPosition, info.point) > _stepDistance && !IsMovingOtherIKFootSolvers() && _lerp >= 1)
            {
                _lerp = 0;
                int direction = _body.InverseTransformPoint(info.point).z > _body.InverseTransformPoint(_newPosition).z ? 1 : -1;
                _newPosition = info.point + (_body.forward * _stepLength * direction) + _footOffset;
                _newNormal = info.normal;
            }
        }

        if (_lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
            tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

            _currentPosition = tempPosition;
            _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
            _lerp += Time.deltaTime * _stepSpeed;
        }
        else
        {
            _oldPosition = _newPosition;
            _oldNormal = _newNormal;
        }
    }

    protected virtual bool IsMovingOtherIKFootSolvers()
    {
        var length = _otherIKFootSolvers.Length;
        for (var i = 0; i < length; ++i)
        {
            if (_otherIKFootSolvers[i].IsMoving())
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_newPosition, 0.5f);
    }
}
