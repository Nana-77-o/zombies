using UnityEngine;

/// <summary> 地雷 </summary>
public class Mine : WeaponBase
{
    [SerializeField, Header("攻撃の中心")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("攻撃半径")]
    float _attackRadius = 1f;
    [SerializeField, Tooltip("アニメーション")]
    Animation _spearAnim;
    [SerializeField, Header("爆発エフェクト")]
    GameObject[] _explosionEffects = default;
    [SerializeField]
    private LayerMask _hitLayer = default;

    bool _isExplosion = false;
    private Damage _exDamage = default;
    private void LateUpdate()
    {
        if (_isExplosion)
        {
            gameObject.SetActive(false);
            _isExplosion = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageApplicable target) && !other.TryGetComponent(out PlayerMoveController playerMove))
        {
            Attack();
        }
    }

    public void StartShot(Damage damage)
    {
        _exDamage = damage;
        ActiveEffect();
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

    public override void Attack()
    {
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius, _hitLayer);   //当たったオブジェクトを全て取得する

        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(_exDamage);
                }
            }
        }
        _isExplosion = true;
        ObjectPoolManager.Instance.Use(_explosionEffects[(int)_exDamage.Attribute]).transform.position = transform.position;
    }
    protected override void ActiveEffect()
    {
        foreach (var effect in _attributeEffects)
        {
            effect.SetActive(false);
        }
        _attributeEffects[(int)_exDamage.Attribute].SetActive(true);
    }
}
