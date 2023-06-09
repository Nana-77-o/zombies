using UnityEngine;

/// <summary>武器　ランス</summary>
public class LanceWeapon : WeaponBase
{
    [SerializeField, Header("攻撃の中心")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("攻撃半径")]
    float _attackRadius = 5f;
    [SerializeField, Header("攻撃間隔")]
    float _attackInterval = 0.5f;

    float _attackTimer = 0f;

    private void OnEnable()
    {
        PlayerInput.SetEnterInput(InputType.Fire3, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire3, this.Attack);
    }

    private void LateUpdate()
    {
        if (_attackInterval > _attackTimer)
        {
            _attackTimer += Time.deltaTime;
        }
    }

    public override void Attack()
    {
        if (_attackInterval > _attackTimer) { return; }
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //当たったオブジェクトを全て取得する

        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(GetDamage());
                }
            }
        }

        _attackTimer = 0;
    }

    private void OnDrawGizmosSelected() //攻撃範囲のギズモを表示する
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenter(), _attackRadius);
    }

    /// <summary>攻撃の中心を取得する</summary>
    /// <returns>攻撃の中心</returns>
    Vector3 GetCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackCenter.z
           + this.transform.up * _attackCenter.y
           + this.transform.right * _attackCenter.x;
        return center;
    }
}
