using UnityEngine;

/// <summary>バブル銃で発射する弾丸のクラス </summary>
public class BubbleBullet : WeaponBullet
{
    [SerializeField, Header("攻撃の中心")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("攻撃半径")]
    float _attackRadius = 1f;
    [SerializeField, Header("着弾時のエフェクト")] 
    ParticleSystem _explosionEffect = default;
    /// <summary>敵ではない爆発対象に着弾したかどうか </summary>
    bool _isHit = false;
    /// <summary>敵に着弾したかどうか </summary>
    bool _isEnemyHit = false;
    Damage _damage = default;

    private void Update()
    {
        if (_explosionEffect.isStopped && _isHit)   //爆発エフェクトが終了したらアクティブ状態をfalseにする
        {
            var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //爆発時に衝突した敵にダメージを与える

            if (0 < hitObjects.Length)
            {
                foreach (var obj in hitObjects)
                {
                    if (obj.TryGetComponent(out IDamageApplicable target))  //敵に着弾
                    {
                        target.AddDamage(_damage);
                        break;
                    }
                }
            }

            _isHit = false;
            gameObject.SetActive(false);
        }

        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //当たったオブジェクトを全て取得する

        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))  //敵に着弾
                {
                    target.AddDamage(_damage);
                    _isEnemyHit = true;
                }

                if (!obj.TryGetComponent(out BubbleBullet bullet))　//他の弾丸以外に着弾
                {
                    _explosionEffect.Play();
                    _isHit = true;
                }
            }

            if (_isEnemyHit)
            {
                _isEnemyHit = false;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected() //攻撃範囲のギズモを表示する
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenter(), _attackRadius);
    }

    public override void StartShot(Damage damage)
    {
        _damage = damage;
        base.StartShot(damage);
    }

    Vector3 GetCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackCenter.z
           + this.transform.up * _attackCenter.y
           + this.transform.right * _attackCenter.x;
        return center;
    }
}
