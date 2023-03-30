using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    /// <summary>キャラクターが受けるダメージ</summary>
    public float _damage { get; }
    /// <summary> 攻撃種 </summary>
    public WeaponType Type { get; }
    /// <summary> 属性種 </summary>
    public AttributeType Attribute { get; }
    /// <summary> 属性値 </summary>
    public float AttributePower { get; }

    public Damage(float nomalDamage, AttributeType attribute = AttributeType.Wut, float attributePower = 1f)
    {
        _damage = nomalDamage;
        Type = WeaponType.Enemy;
        Attribute = attribute;
        AttributePower = attributePower;
    }
    public Damage(CharacterParameter character, WeaponParameter weapon)
    {
        _damage = character.DefaultPower + weapon.Damage;
        Type = weapon.WeaponType;
        Attribute = weapon.AttributeType;
        AttributePower = weapon.AttributePower + character.AttributePower[(int)Attribute];
    }
    public Damage(CharacterParameter character, WeaponParameter weapon, AttributeType attribute)
    {
        _damage = character.DefaultPower + weapon.Damage;
        Type = weapon.WeaponType;
        Attribute = attribute;
        AttributePower = weapon.AttributePower + character.AttributePower[(int)Attribute];
    }
}
