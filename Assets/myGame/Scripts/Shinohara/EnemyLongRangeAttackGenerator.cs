using UnityEngine;

/// <summary>
/// 敵遠距離攻撃のオブジェクトを生成する為クラス
/// 遠距離攻撃をさせたい敵にこのクラスを追加する
/// </summary>
public class EnemyLongRangeAttackGenerator : WeaponBase
{
    [SerializeField, Header("攻撃間隔")]
    float _attackInterval = 10f;
    [SerializeField, Header("生成オブジェクト")]
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

    /// <summary>敵遠距離攻撃を発生させる </summary>
    /// <param name="generatePosition">生成位置</param>
    /// /// <param name="arrivalPosition">着弾位置</param>
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
