using System.Collections;
using UnityEngine;

/// <summary> 地雷生成クラス</summary>
public class LandminesWeapon : WeaponBase
{
    [SerializeField, Header("生成インターバル")]
    float _attackInterval = 3f;
    [SerializeField, Tooltip("生成する地雷")]
    Mine _minePrefab = default;
    [Tooltip("インターバルbool")]
    bool _intervalBool = false;

    float _attackTimer = 1f;
    /// <summary> 攻撃インターバル </summary>
    public float AttackTimer { get => _attackTimer; }

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
        if (_intervalBool)
        {
            _attackTimer += Time.deltaTime;
        }

        if (_attackInterval <= _attackTimer)  //一定時間後地雷を再配置可能にする
        {
            _attackTimer = 0;
            _intervalBool = false;
        }
    }

    public override void Attack()
    {
        if (_intervalBool) { return; }
        if (IsCharge == true || UseCharge() == false)
        {
            return;
        }
        var mineGameObject = ObjectPoolManager.Instance.Use(_minePrefab.gameObject, transform.position);
        var mine = mineGameObject.GetComponent<Mine>();
        mine.Initialize(_owner);
        mine.StartShot(GetDamage());
        _intervalBool = true;
    }
}
