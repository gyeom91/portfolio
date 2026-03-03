using UnityEngine;

public class Character : Pawn
{
    protected Rigidbody _rigidbody { get; private set; }
    protected Animator _animator { get; private set; }
    protected Vector3 _moveDirection;
    protected virtual float _posSpeed { get; }

    [SerializeField] private float _rotSpeed;

    protected override void OnBehaviour()
    {
        base.OnBehaviour();

        //_animator.SetFloat("MoveX", _moveDirection.x);
        //_animator.SetFloat("MoveY", _moveDirection.z);
    }

    protected override void OnFixedBehaviour()
    {
        base.OnFixedBehaviour();

        if (_moveDirection != Vector3.zero)
        {
            var rotation = Quaternion.Slerp(
                _rigidbody.rotation, 
                Quaternion.LookRotation(_moveDirection), 
                _rotSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(rotation);
        }

        var pos = _rigidbody.position;
        var dir = pos + (_moveDirection * _posSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(dir);

        //if (_moveDirection != Vector3.zero)
        //    _rigidbody.MoveRotation(Quaternion.LookRotation(_moveDirection));

        //var pos = _rigidbody.position;
        //var dir = pos + (_moveDirection * _speed * Time.deltaTime);
        //_rigidbody.MovePosition(dir);
    }

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }
}
