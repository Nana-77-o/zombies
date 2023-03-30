using Perapera_Puroto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SamplePlayer : CharacterBase
{
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    [SerializeField]
    private InGameUIController _uiController = default;
    [SerializeField]
    private int _hpMaxCount = 3;
    [SerializeField]
    private int _overDamageCount = 1;
    [SerializeField]
    private float _damageInterval = 0.5f;
    private float _damageTimer = 0;
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
    private void Update()
    {
        if (_damageTimer > 0)
        {
            _damageTimer -= Time.deltaTime;
        }
    }

    protected override void Initialization()
    {
        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _isInitalize = true;
        _parameter.CurrentHP = _parameter.MaxHP;
        _hpCount = _hpMaxCount;
        _uiController.SetHPView(_hpMaxCount);
        DelHPUpDate += _uiController.ShowHPView;
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
        _damageTimer = _damageInterval;
    }
}