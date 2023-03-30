using System.Collections;
using UnityEngine;

/// <summary> �G�̉������U���Ŏg�p����U���I�u�W�F�N�g���Ǘ�����N���X  </summary>
public class EnemyLongRangeAttack : WeaponBullet
{
    [SerializeField, Header("�؋󎞊�")]
    float _flightTime = 2;
    [SerializeField, Header("�ړ����x")]
    float _moveSpeed = 3;
    /// <summary>�w�肵���ʒu�ɓ���</summary>
    bool _isArrival = false;
    /// <summary>�����ʒu�ƒ��e�ʒu�̋��� </summary>
    float _distance = 0f;
    /// <summary>���ˈʒu </summary>
    Vector3 _firePoint = Vector3.zero;

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
        if (_isArrival)     //�������z�������Ɉړ�������
        {
            transform.position += transform.forward * _distance * Time.deltaTime;
        }
    }

    /// <summary>
    /// �������U���𔭓�����
    /// �������U�����s�����ɌĂяo��
    /// </summary>
    /// <param name="targetPos">���e�ʒu</param>
    public override void EnemyLongAttack(Damage damage, Vector3 generatePoint, Vector3 arrivalPoint)
    {
        _lifeTimer = 0;
        base.EnemyLongAttack(damage, generatePoint, arrivalPoint);
        MoveInitialization(generatePoint, arrivalPoint);
        ActiveEffect();
    }

    /// <summary> �ړ��J�n���̏����ݒ� </summary>
    /// <param name="arrivalPoint">���e�ʒu</param>
    public void MoveInitialization(Vector3 generatePoint,  Vector3 arrivalPoint)
    {
        _firePoint = generatePoint;
        _distance = Vector3.Distance(generatePoint, arrivalPoint);
        StartCoroutine(Move(arrivalPoint));
    }

    /// <summary>�U���I�u�W�F�N�g(���g)�̈ړ����� </summary>
    /// <param name="targetPos">���e�ʒu</param>
    IEnumerator Move(Vector3 targetPos)
    {
        var diffY = (targetPos - _firePoint).y; // y������
        var firstSpeed = (diffY - Physics.gravity.y * 0.5f * _flightTime * _flightTime) / _flightTime; // �����x

        // �����^��
        for (var t = 0f; t < _flightTime; t += Time.deltaTime * _moveSpeed)     //�؋󎞊ԕ���
        {
            var currentPos = Vector3.Lerp(_firePoint, targetPos, t / _flightTime);
            currentPos.y = _firePoint.y + firstSpeed * t + 0.5f * Physics.gravity.y * t * t;
            transform.position = currentPos;
            transform.forward = targetPos - transform.position;

            yield return null;
        }

        _isArrival = true;
    }
}