using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// 近接攻撃用のクラス
/// ナイフやランスなど
/// </summary>
public class MeleeAttackWeapon : WeaponBase
{
    [SerializeField, Header("攻撃の中心")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("攻撃半径")]
    float _attackRadius = 1f;
    [SerializeField, Header("攻撃インターバル")]
    float _attackInterval = 0.5f;
    [SerializeField, Header("まとめて攻撃")]
    bool _multiAttack = false;
    [SerializeField]
    Animator _attackAnime = default;
    [SerializeField]
    int _hitCount = 1;
    string _attackName = "Attack";
    float _attackTimer = 0f;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        Initialize(Character.GetParameter());
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ActiveEffect();
        PlayerInput.SetEnterInput(InputType.Fire1, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire1, this.Attack);
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
        if (IsCharge == true || UseCharge() == false)
        {
            return;
        }
        if (_attackAnime != null)
        {
            _attackAnime.Play(_attackName);
        }
        if (_playerAnime != null)
        {
            _playerAnime.Attack();
        }
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //当たったオブジェクトを全て取得する

        if (0 < hitObjects.Length)
        {
            //敵一体のみに攻撃時はbreak
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    for (int i = 0; i < _hitCount; i++)
                    {
                        target.AddDamage(GetDamage());
                    }
                    if (!_multiAttack)
                    {
                        break;
                    }
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
