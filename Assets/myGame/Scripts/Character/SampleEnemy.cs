using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : CharacterBase
{
    private static readonly int ATTRIBUTE_TYPES = 4;
    private static readonly int RANDAM_RANGE = 25;
    private static readonly int RESISTANCES = 100;
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    [SerializeField]
    private NaviEnemy _move = default;
    [SerializeField]
    private EnemyAttack _attack = default;

    public event Action DelDead = default;
    private bool _isActive = false;
    private float _attackTimer = 0;
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
        int type = UnityEngine.Random.Range(0, RANDAM_RANGE);
        if (type < ATTRIBUTE_TYPES)
        {
            Instantiate(_addCounter.GetAttributeEffect((AttributeType)type), transform);
            for (int i = 0; i < ATTRIBUTE_TYPES; i++)
            {
                if (type == i)
                {
                    continue;
                }
                _parameter.Resistances[i] = RESISTANCES;
            }
        }
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
        _isActive = false;
    }
    public override void AddDamage(Damage damage)
    {
        if (_isInitalize == false || damage.Type == WeaponType.Enemy || _parameter.CurrentHP <= 0) { return; }
        _hitEffect?.Play();
        _addCounter.AddDamageCount(damage);
        _damageTaken = damage;
        var calDamage = DamageCalculator.GetDamage(_parameter, damage);
        DamageView.Show(transform.position + Vector3.up, calDamage);
        _parameter.CurrentHP -= calDamage;
    }
}
