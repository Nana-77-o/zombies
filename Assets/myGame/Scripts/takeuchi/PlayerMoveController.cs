using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{
    public float _moveSpeed = 2f;
    [SerializeField]
    private Transform _body = default;
    [SerializeField]
    private WallChecker _forwardChecker = default;
    [SerializeField]
    private WallChecker _leftChecker = default;
    [SerializeField]
    private WallChecker _rightChecker = default;
    [SerializeField]
    private WallChecker _backChecker = default;
    private Rigidbody _rigidbody;
    private float _g = -0.5f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 dir = (Vector3.right * PlayerInput.InputVector.x + Vector3.forward * PlayerInput.InputVector.y).normalized * _moveSpeed;

        if (dir != Vector3.zero && _body != null)
        {
            _body.forward = dir;
        }
        if (_forwardChecker is not null && _backChecker is not null && _leftChecker is not null && _rightChecker is not null)
        {
            if (dir.x > 0 && !_rightChecker.IsWalled() || dir.x < 0 && !_leftChecker.IsWalled())
            {
                dir.x = 0;
            }
            if (dir.z > 0 && !_forwardChecker.IsWalled() || dir.z < 0 && !_backChecker.IsWalled())
            {
                dir.z = 0;
            }
        }
        dir.y = _rigidbody.velocity.y + _g;
        _rigidbody.velocity = dir;
    }
}
