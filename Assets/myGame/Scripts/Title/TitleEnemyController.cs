using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TitleEnemyController : MonoBehaviour
{
    // switch��RigidBody���g�p����̂͏d���炵���̂ł��ƂŏC������
    [SerializeField]
    private Vector3[] _movePositions = default;
    [SerializeField]
    private float _moveSpeed = 1f;

    private Rigidbody _rigidbody;
    [SerializeField]
    private int _nextIndex = 0;

    #region Unity Message
    private void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Move()
    {
        var dir = _movePositions[_nextIndex] - transform.position;
        _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z).normalized * _moveSpeed;

        Transition();
    }
    private void Transition()
    {
        // �Ώۂ܂ň�苗���߂Â������A
        // �Ώۂ��X�V����B
        if (Mathf.Abs(_movePositions[_nextIndex].x - transform.position.x) < 0.3f &&
            Mathf.Abs(_movePositions[_nextIndex].z - transform.position.z) < 0.3f)
        {
            _nextIndex++;
            _nextIndex %= _movePositions.Length;
        }
    }
}
