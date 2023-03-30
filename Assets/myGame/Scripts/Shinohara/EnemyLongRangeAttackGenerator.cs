using UnityEngine;

/// <summary>
/// �G�������U���̃I�u�W�F�N�g�𐶐�����׃N���X
/// �������U�������������G�ɂ��̃N���X��ǉ�����
/// </summary>
public class EnemyLongRangeAttackGenerator : WeaponBase
{
    [SerializeField, Header("�U���Ԋu")]
    float _attackInterval = 10f;
    [SerializeField, Header("�����I�u�W�F�N�g")]
    EnemyLongRangeAttack _attackObject = default;

    float _attackTimer = 0f;

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackInterval <= _attackTimer)
        {
            Attack();
            _attackTimer = 0f;
        }
    }

    /// <summary>�G�������U���𔭐������� </summary>
    /// <param name="generatePosition">�����ʒu</param>
    /// /// <param name="arrivalPosition">���e�ʒu</param>
    public void GenerateAttack(Vector3 generatePosition, Vector3 arrivalPosition)
    {
        var go = BulletPool.Instance.GetBullet(_attackObject);
        go.EnemyLongAttack(GetDamage(), generatePosition, arrivalPosition);
    }

    public override void Attack()
    {
        GenerateAttack(transform.position, NavigationManager.Instance.DefaultTarget.position);
    }
}
