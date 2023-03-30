using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算を行う
/// </summary>
public static class DamageCalculator
{
    public static float GetDamage(CharacterParameter parameter,Damage damage)
    {
        if(damage.AttributePower <= parameter.Resistances[(int)damage.Attribute])
        {
            return damage._damage * 0;
        }
        return damage._damage * (damage.AttributePower - parameter.Resistances[(int)damage.Attribute]);
    }
    public static float GetDamage(Damage damage)
    {
        return damage._damage;
    }
}
