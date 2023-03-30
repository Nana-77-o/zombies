using System.Collections;
using UnityEngine;

/// <summary> �n�������N���X</summary>
public class LandminesWeapon : WeaponBase
{
    [SerializeField, Header("�����C���^�[�o��")]
    float _attackInterval = 3f;
    [SerializeField, Tooltip("��������n��")]
    Mine _minePrefab = default;
    [Tooltip("�C���^�[�o��bool")]
    bool _intervalBool = false;

    float _attackTimer = 1f;
    /// <summary> �U���C���^�[�o�� </summary>
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

        if (_attackInterval <= _attackTimer)  //��莞�Ԍ�n�����Ĕz�u�\�ɂ���
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
