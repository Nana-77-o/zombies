using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : CharacterBase
{
    DestructionManager _destructionmanager = default;

    public DestructionManager Destructionmanager { get => _destructionmanager; set => _destructionmanager = value; }

    protected override void DeadCharacter()
    {
        gameObject.SetActive(false);
        _destructionmanager.GameobjCount();
    }
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
        Debug.Log("EnemyDamage");
    }
}
