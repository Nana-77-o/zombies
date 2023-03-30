using Perapera_Puroto;
using System.Collections;
using UnityEngine;

public class PlayerParameter : CharacterBase
{
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    [SerializeField]
    private InGameUIController _uiController = default;
    [SerializeField]
    private float _damageInterval = 0.5f;
    [SerializeField]
    private ChargeGun _chargeGun = default;
    [SerializeField]
    private int _hpMaxCount = 3;
    [SerializeField]
    private int _overDamageCount = 1;
    [SerializeField]
    private WeaponParameter[] _weaponParameters = null;
    [Header("�p���I�ȃp���[�A�b�v")]
    [SerializeField]
    float _attack = 2;
    [SerializeField]
    float _speed = 2;
    [SerializeField]
    float _chargeSpeed = 2;
    [Header("�ォ��i�C�t,�����X,�n��,�e")]
    [SerializeField, Tooltip("�c�e���̏���l")]
    int[] _chargCount = default;
    [Header("�ꎞ�I�ȃp���[�A�b�v")]
    [SerializeField, Tooltip("�p������")]
    float _time = 3;
    [SerializeField]
    int _addAttack = 5;
    [SerializeField]
    int _addSpeed = 5;
    [SerializeField]
    int _addChargeSpeed = 5;
    [Header("�ォ��i�C�t,�����X,�n��,�e")]
    [SerializeField, Tooltip("�ꎞ�I�Ȏc�i���̏���l")]
    int[] _addChargeCont = default;

    private float _damageTimer = 0;
    private bool _item = false;
    private PlayerMoveController _moveController;
    private int _hpCount = 0;
    private System.Action<int> DelHPUpDate = default;

    public int HPCount
    {
        get => _hpCount;
        private set
        {
            _hpCount = value;
            if (_hpCount <= 0)
            {
                DeadCharacter();
                _hpCount = 0;
            }
            DelHPUpDate?.Invoke(_hpCount);
        }
    }

    public bool Item { get => _item; set => _item = value; }

    protected override void Initialization()
    {
        _moveController = GetComponent<PlayerMoveController>();

        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _isInitalize = true;
        _parameter.CurrentHP = _parameter.MaxHP;
        _hpCount = _hpMaxCount;
        _uiController.SetHPView(_hpMaxCount);
        DelHPUpDate += _uiController.ShowHPView;
    }

    private void Update()
    {
        if (_damageTimer > 0)
        {
            _damageTimer -= Time.deltaTime;
        }
    }

    protected override void DeadCharacter()
    {
        GameManager.Instance.GameOver();
        Debug.Log("GameOver");
        gameObject.SetActive(false);
    }
    public override void AddDamage(Damage damage)
    {
        if (_isInitalize == false || damage.Type != WeaponType.Enemy || HPCount <= 0) { return; }
        if (_damageTimer > 0) { return; }
        _hitEffect?.Play();
        if (DamageCalculator.GetDamage(_parameter, damage) >= _parameter.MaxHP)
        {
            for (int i = 0; i < _overDamageCount; i++)
            {
                _hpCount--;
            }
        }
        HPCount--;
    }

    /// <summary>
    /// �p���[�A�b�v�A�C�e���l��
    /// </summary>
    public void PowerUPItem()
    {
        switch (BossMissionManager.Instance.Type)
        {
            case BossMissionManager.PoewrUPType.AlwaysSpeed:
                _moveController._moveSpeed += _speed;
                break;
            case BossMissionManager.PoewrUPType.AlwaysAttack:
                _parameter[ParamID.AttackPower] += _attack;
                break;
            case BossMissionManager.PoewrUPType.AlwaysChargeSpeed:
                _parameter.AttackSpeed += _chargeSpeed;
                break;
            case BossMissionManager.PoewrUPType.AlwaysChargCount:
                GansCountUP(_chargCount);
                break;

            case BossMissionManager.PoewrUPType.TemporarySpeed:
                _moveController._moveSpeed += _addSpeed;
                StartCoroutine(Temporary());
                break;
            case BossMissionManager.PoewrUPType.TemporaryAttack:
                _parameter[ParamID.AttackPower] += _addAttack;
                StartCoroutine(Temporary());
                break;
            case BossMissionManager.PoewrUPType.TemporaryChargeSpeed:
                StartCoroutine(Temporary());
                _parameter.AttackSpeed += _addChargeSpeed;
                break;
            case BossMissionManager.PoewrUPType.TemporaryChargCount:
                GansCountUP(_addChargeCont);
                StartCoroutine(Temporary());
                break;
        }
    }

    /// <summary>
    /// ����̎c�i���̏���lUP
    /// </summary>
    /// <param name="arry">�グ�����l�̔z��</param>
    /// <param name="sign">1 = �����Z,-1 = �����Z</param>
    void GansCountUP(int[] arry, int sign = 1)
    {
        for (int i = 0; i < _weaponParameters.Length; i++)
        {
            _weaponParameters[i].AddCount += arry[i] * sign;
        }
    }
    /// <summary>
    /// �ꎞ�I�ɏグ���l��߂�
    /// </summary>
    IEnumerator Temporary()
    {
        Item = true;
        yield return new WaitForSeconds(_time);
        switch (BossMissionManager.Instance.Type)
        {
            case BossMissionManager.PoewrUPType.TemporarySpeed:
                _moveController._moveSpeed -= _addSpeed;
                break;
            case BossMissionManager.PoewrUPType.TemporaryAttack:
                _parameter[ParamID.AttackPower] -= _addAttack;
                break;
            case BossMissionManager.PoewrUPType.TemporaryChargeSpeed:
                _parameter.AttackSpeed -= _addChargeSpeed;
                break;
            case BossMissionManager.PoewrUPType.TemporaryChargCount:
                GansCountUP(_addChargeCont, -1);
                break;
        }

        Item = false;
    }

}
