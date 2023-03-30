using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : CharacterBase
{
    GameObject _player;

    public GameObject Player { get => _player; set => _player = value; }
    protected override void Initialization()
    {
        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _isInitalize = true;
    }
    public override void AddDamage(Damage damage)
    {
        if (_isInitalize == false || damage.Type == WeaponType.Enemy) { return; }
        _parameter.CurrentHP += DamageCalculator.GetDamage(_parameter, damage);
        _player.GetComponent<PlayerParameter>().PowerUPItem();
        gameObject.SetActive(false);
    }
}
