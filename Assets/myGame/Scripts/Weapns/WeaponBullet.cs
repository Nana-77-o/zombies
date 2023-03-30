using UnityEngine;

/// <summary>弾丸の操作クラス</summary>
public class WeaponBullet : MonoBehaviour
{
    [SerializeField, Header("生存時間")]
    protected float _lifeTime = 2f;
    [SerializeField, Header("移動速度"), Range(0.01f, 1)]
    protected float _speed = 5f;
    [SerializeField, Header("当たり判定を行うまでのフレーム数")]
    private int _rayFrame = 5;
    [SerializeField, Header("衝突するレイヤー")]
    private LayerMask _hitLayer = default;
    [SerializeField, Header("衝突範囲")]
    protected float _radius = 2f;
    [SerializeField, Header("衝突回避時間")]
    protected float _anHitTime = 0f;
    [SerializeField, Header("衝突回数")]
    protected int _maxHitCount = 0;
    [SerializeField]
    protected GameObject _effect = default;
    [SerializeField]
    private GameObject[] _attributeEffects = default;
    /// <summary>現在のフレーム </summary>
    private int _frameCount = 0;
    private Damage _damage = default;
    /// <summary>前回当たり判定を行った位置</summary>
    private Vector3 _beforePos = default;
    protected float _lifeTimer = default;
    protected int _hitCount = 0;
    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, _speed);
        if (_lifeTimer < _anHitTime)
        {
            return;
        }
        HitCheck();
    }

    /// <summary>rayを使用した当たり判定 </summary>
    private void HitCheck()
    {
        if (gameObject.activeInHierarchy == false || _speed <= 0) { return; }

        _frameCount++;

        if (_frameCount < _rayFrame) { return; }
        _frameCount = 0;

        if (Physics.SphereCast(_beforePos, _radius, transform.forward, out RaycastHit hit, Vector3.Distance(_beforePos, transform.position), _hitLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageApplicable target))     //敵にダメージを与える
            {
                target.AddDamage(_damage);
            }
            ObjectPoolManager.Instance.Use(_effect).transform.position = transform.position;
            _hitCount--;
            if (_hitCount < 0)
            {
                gameObject.SetActive(false);
            }
        }

        _beforePos = transform.position;
    }

    ///// <summary> ダメージを設定し、移動開始する </summary>
    public virtual void StartShot(Damage damage)
    {
        _damage = damage;
        _lifeTimer = 0;
        ActiveEffect();
        gameObject.SetActive(true);
        _hitCount = _maxHitCount;
    }

    /// <summary>敵遠距離攻撃</summary>
    /// <param name="arrivalPoint">着弾位置<param>
    public virtual void EnemyLongAttack(Damage damage, Vector3 generatePoint, Vector3 arrivalPoint)
    {
        _damage = damage;
        gameObject.SetActive(true);
        _hitCount = _maxHitCount;
    }
    protected void ActiveEffect()
    {
        foreach (var effect in _attributeEffects)
        {
            effect.SetActive(false);
        }
        if ((int)_damage.Attribute >= _attributeEffects.Length)
        {
            return;
        }
        _attributeEffects[(int)_damage.Attribute].SetActive(true);
    }
}
