using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : CharacterBase
{
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    [SerializeField]
    private NaviUser _move = default;
    [SerializeField]
    private GameObject _bossBody = default;
    [SerializeField]
    private EnemyAttack _attack = default;
    [SerializeField]
    private GameObject _deadEffect = default;
    [SerializeField]
    private GameObject[] _auraEffects = default;

    [Tooltip("移動速度の増加倍率")]
    [SerializeField]
    private float _moveMagnification = 1.1f;

    [Tooltip("攻撃力の増加倍率")]
    [SerializeField]
    private float _attackMagnification = 1.1f;

    [Tooltip("攻撃範囲の増加倍率")]
    [SerializeField]
    private float _attackRangeMagnification = 1.1f;

    [Tooltip("攻撃速度の増加倍率")]
    [SerializeField]
    private float _attackSpeedMagnification = 1.1f;

    [SerializeField]
    private int _zombieRank;

    public event Action DelDead = default;
    public Transform Body = default;
    private bool _isActive = false;
    private float _attackTimer = 0;
    private bool _isEating = false;
    private int[] _paramRank = default;
    private int _rankCount = 0;
    private void OnEnable()
    {
        if (_isInitalize == false) { return; }
        _parameter.CurrentHP = _paramData.MaxHp;
        _isActive = true;
    }
    private void Update()
    {
        if (_isActive == false) { return; }
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }
        if (Vector3.Distance(NavigationManager.Instance.DefaultTarget.position, _attack.transform.position) <= _attack.AttackRadius)
        {
            _attack.Attack();
            _attackTimer = _parameter.AttackSpeed;
        }
    }
    protected override void Initialization()
    {
        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _move.SetSpeed(_parameter.MoveSpeed);
        _isInitalize = true;
        _isActive = true;
        _attack.SetDamage(_parameter);
    }
    protected override void DeadCharacter()
    {
        _addCounter.AddCount(_damageTaken, _charaID);
        DelDead?.Invoke();
        _move.StopMove();
        ObjectPoolManager.Instance.Use(_deadEffect).transform.position = transform.position;
        _isActive = false;
        _bossBody.SetActive(false);
    }
    public void SetParamRank(int[] rank)
    {
        _paramRank = rank;
    }
    public override void AddDamage(Damage damage)
    {
        if (_isInitalize == false || damage.Type == WeaponType.Enemy || _parameter.CurrentHP <= 0) { return; }
        _hitEffect?.Play();
        _damageTaken = damage;
        _addCounter.AddDamageCount(damage);
        var calDamage = DamageCalculator.GetDamage(_parameter, damage);
        DamageView.Show(transform.position + Vector3.up, calDamage);
        _parameter.CurrentHP -= calDamage;
    }
    /// <summary>
    /// 指定のランクのパラメータを更新する
    /// </summary>
    /// <param name="charaParam"></param>
    public void UpdateParam(CharaParamData charaParam, BossParamData.RankID targetRank = BossParamData.RankID.S)
    {
        for (int i = 0; i < CharaParamData.PARAM_NUMBER; i++)
        {
            if (_paramRank[i] == (int)targetRank)
            {
                _parameter[(ParamID)i] = charaParam[(ParamID)i];
            }
        }
    }
    /// <summary>
    /// 共食いした際に呼び出してね
    /// </summary>
    public void Cannibal(CharaParamData charaParamData)
    {
        _parameter = new CharacterParameter(charaParamData);
        _parameter.DelDead += DeadCharacter;
        _move.SetSpeed(_parameter.MoveSpeed);
        _isInitalize = true;
        _isActive = true;
        _attack.SetDamage(_parameter);
    }
    /// <summary>
    /// 共食いの計算式とパラメーターの変更
    /// </summary>
    /// <param name="eater">食う側のBossEnemyクラス</param>
    /// <param name="dinner">食われる側のBossEnemyクラス</param>
    private void PowerDrain(BossEnemy dinner)
    {
        this._parameter.DefaultPower = Mathf.Max(this._parameter.DefaultPower, dinner._parameter.DefaultPower) * _attackMagnification;
        this._parameter.MoveSpeed = Mathf.Max(this._parameter.MoveSpeed, dinner._parameter.MoveSpeed) * _moveMagnification;
        this._parameter.MaxHP += dinner._parameter.MaxHP;
        this._parameter.CurrentHP += dinner._parameter.CurrentHP;
        this._parameter.AttackRange = Mathf.Max(this._parameter.AttackRange, dinner._parameter.AttackRange) * _attackRangeMagnification;
        this._parameter.AttackSpeed = Mathf.Max(this._parameter.AttackSpeed, dinner._parameter.AttackSpeed) * _attackSpeedMagnification;
        for (int resistances = 0; resistances < CharacterParameter.ATTRIBUTE_TYPES; resistances++)
        {
            this._parameter.Resistances[resistances] += dinner._parameter.Resistances[resistances];//各種族生耐性
            this._parameter.AttributePower[resistances] += dinner._parameter.AttributePower[resistances];//各種属性攻撃力
        }
        _isEating = false;
        _zombieRank++;
        dinner.Body.gameObject.SetActive(false);
        if (_rankCount < _auraEffects.Length)
        {
            _auraEffects[_rankCount].SetActive(true);
            _rankCount++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_isEating && other.gameObject.TryGetComponent(out BossEnemy bossEnemy))
        {
            if (bossEnemy._isEating)
            {
                return;
            }
            _isEating = true;
            bossEnemy._isEating = true;
            if (bossEnemy._zombieRank <= this._zombieRank)
            {
                PowerDrain(bossEnemy);
            }
            else
            {
                bossEnemy.PowerDrain(this);
            }
            ObjectPoolManager.Instance.Use(_deadEffect).transform.position = transform.position;
        }
    }

}
