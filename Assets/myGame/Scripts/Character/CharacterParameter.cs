using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// キャラクターのパラメータクラス
/// </summary>
public class CharacterParameter
{
    /// <summary> 武器種数 </summary>
    public static readonly int WEAPON_TYPES = Enum.GetValues(typeof(WeaponType)).Length;
    /// <summary> 属性種数 </summary>
    public static readonly int ATTRIBUTE_TYPES = Enum.GetValues(typeof(AttributeType)).Length;
    #region Private Field
    /// <summary> 最大耐久値 </summary>
    private int _maxHP;
    private float _hp = default;
    private float _power = default;
    private float _moveSpeed = default;
    private float _attackRange = default;
    private float _attackSpeed = default;
    private float[] _attributePower = default;
    private float[] _resistances = default;

    #endregion

    public int MaxHP
    {
        get => _maxHP;
        set
        {
            _maxHP = value;
            DelUpdateParameter?.Invoke();
            if (_maxHP <= 0)
            {
                DelDead?.Invoke();
            }
        }
    }

    /// <summary> 耐久値 </summary>
    public float CurrentHP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp > _maxHP)
            {
                _hp = _maxHP;
            }
            DelUpdateParameter?.Invoke();
            if (_hp <= 0)
            {
                DelDead?.Invoke();
            }
        }
    }
    /// <summary> 基礎攻撃力 </summary>
    public float DefaultPower
    {
        get => _power;
        set
        {
            _power = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> 移動速度 </summary>
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            _moveSpeed = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> 攻撃範囲 </summary>
    public float AttackRange
    {
        get => _attackRange;
        set
        {
            _attackRange = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> 攻撃速度 </summary>
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            _attackSpeed = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> 攻撃属性値 </summary>
    public float[] AttributePower
    {
        get => _attributePower;
        set
        {
            _attributePower = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> 属性耐性値 </summary>
    public float[] Resistances
    {
        get => _resistances;
        set
        {
            _resistances = value;
            DelUpdateParameter?.Invoke();
        }
    }

    /// <summary> Parameter更新時のイベント </summary>
    public event Action DelUpdateParameter = default;
    /// <summary> 死亡時に呼ばれるイベント </summary>
    public event Action DelDead = default;
    /// <summary>
    /// パラメータコンストラクタ
    /// </summary>
    /// <param name="moveSpeed">移動速度</param>
    /// <param name="maxHp">最大HP</param>
    /// <param name="attackRange">最大チャージ量</param>
    /// <param name="attackSpeed">チャージ速度</param>
    /// <param name="resistanceFreude">"喜"耐性</param>
    /// <param name="resistanceWut">"怒"耐性</param>
    /// <param name="resistanceKummer">"哀"耐性</param>
    /// <param name="resistanceEinfach">"楽"耐性</param>
    /// <param name="attributeFreude">"喜"属性値</param>
    /// <param name="attributeWut">"怒"属性値</param>
    /// <param name="attributeKummer">"哀"属性値</param>
    /// <param name="attributeEinfach">"楽"属性値</param>
    public CharacterParameter(float moveSpeed = 5f, int maxHp = 100, float attackRange = 10, float attackPower = 1, float attackSpeed = 5f,
        float resistanceFreude = 0, float resistanceWut = 0, float resistanceKummer = 0, float resistanceEinfach = 0,
        float attributeFreude = 0, float attributeWut = 0, float attributeKummer = 0, float attributeEinfach = 0)
    {
        _maxHP = maxHp;
        _hp = maxHp;
        _attackRange = attackRange;
        _power = attackPower;
        _moveSpeed = moveSpeed;
        _attackSpeed = attackSpeed;
        _resistances = new float[ATTRIBUTE_TYPES];
        _resistances[(int)AttributeType.Freude] = resistanceFreude;
        _resistances[(int)AttributeType.Wut] = resistanceWut;
        _resistances[(int)AttributeType.Kummer] = resistanceKummer;
        _resistances[(int)AttributeType.Einfach] = resistanceEinfach;
        _attributePower = new float[ATTRIBUTE_TYPES];
        _attributePower[(int)AttributeType.Freude] = attributeFreude;
        _attributePower[(int)AttributeType.Wut] = attributeWut;
        _attributePower[(int)AttributeType.Kummer] = attributeKummer;
        _attributePower[(int)AttributeType.Einfach] = attributeEinfach;
    }
    public CharacterParameter(CharaParamData data)
    {
        _maxHP = data.MaxHp;
        _hp = data.MaxHp;
        _power = data.AttackPower;
        _attackRange = data.AttackRange;
        _moveSpeed = data.MoveSpeed;
        _attackSpeed = data.AttackSpeed;
        _resistances = new float[ATTRIBUTE_TYPES];
        _resistances[(int)AttributeType.Freude] = data.ResistanceFreude;
        _resistances[(int)AttributeType.Wut] = data.ResistanceWut;
        _resistances[(int)AttributeType.Kummer] = data.ResistanceKummer;
        _resistances[(int)AttributeType.Einfach] = data.ResistanceEinfach;
        _attributePower = new float[ATTRIBUTE_TYPES];
        _attributePower[(int)AttributeType.Freude] = data.AttributeFreude;
        _attributePower[(int)AttributeType.Wut] = data.AttributeWut;
        _attributePower[(int)AttributeType.Kummer] = data.AttributeKummer;
        _attributePower[(int)AttributeType.Einfach] = data.AttributeEinfach;
    }
    /// <summary>
    /// パラメータ取得・更新用インデクサー
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float this[ParamID index]
    {
        get
        {
            switch (index)
            {
                case ParamID.MaxHp:
                    return _maxHP;
                case ParamID.MoveSpeed:
                    return _moveSpeed;
                case ParamID.AttackPower:
                    return _power;
                case ParamID.AttackRange:
                    return _attackRange;
                case ParamID.AttackSpeed:
                    return _attackSpeed;
                case ParamID.ResistanceFreude:
                    return _resistances[(int)AttributeType.Freude];
                case ParamID.ResistanceWut:
                    return _resistances[(int)AttributeType.Wut];
                case ParamID.ResistanceKummer:
                    return _resistances[(int)AttributeType.Kummer];
                case ParamID.ResistanceEinfach:
                    return _resistances[(int)AttributeType.Einfach];
                case ParamID.AttributeFreude:
                    return _attributePower[(int)AttributeType.Freude];
                case ParamID.AttributeWut:
                    return _attributePower[(int)AttributeType.Wut];
                case ParamID.AttributeKummer:
                    return _attributePower[(int)AttributeType.Kummer];
                case ParamID.AttributeEinfach:
                    return _attributePower[(int)AttributeType.Einfach];
                default:
                    return -1;
            }
        }
        set
        {
            switch (index)
            {
                case ParamID.MaxHp:
                    MaxHP = (int)value;
                    return;
                case ParamID.MoveSpeed:
                    MoveSpeed = value;
                    return;
                case ParamID.AttackPower:
                    DefaultPower = value;
                    return;
                case ParamID.AttackRange:
                    AttackRange = value;
                    return;
                case ParamID.AttackSpeed:
                    AttackSpeed = value;
                    return;
                case ParamID.ResistanceFreude:
                    Resistances[(int)AttributeType.Freude] = value;
                    return;
                case ParamID.ResistanceWut:
                    Resistances[(int)AttributeType.Wut] = value;
                    return;
                case ParamID.ResistanceKummer:
                    Resistances[(int)AttributeType.Kummer] = value;
                    return;
                case ParamID.ResistanceEinfach:
                    Resistances[(int)AttributeType.Einfach] = value;
                    return;
                case ParamID.AttributeFreude:
                    AttributePower[(int)AttributeType.Freude] = value;
                    return;
                case ParamID.AttributeWut:
                    AttributePower[(int)AttributeType.Wut] = value;
                    return;
                case ParamID.AttributeKummer:
                    AttributePower[(int)AttributeType.Kummer] = value;
                    return;
                case ParamID.AttributeEinfach:
                    AttributePower[(int)AttributeType.Einfach] = value;
                    return;
                default:
                    return;
            }
        }
    }
}
public enum ParamID
{
    MaxHp,
    MoveSpeed,
    AttackPower,
    AttackRange,
    AttackSpeed,
    ResistanceFreude,
    ResistanceWut,
    ResistanceKummer,
    ResistanceEinfach,
    AttributeFreude,
    AttributeWut,
    AttributeKummer,
    AttributeEinfach,
}
[Serializable]
public struct CharaParamData
{
    public const int PARAM_NUMBER = 13;
    [Header("移動速度")]
    public float MoveSpeed;
    [Header("最大HP")]
    public int MaxHp;
    [Header("攻撃力")]
    public float AttackPower;
    [Header("攻撃範囲")]
    public float AttackRange;
    [Header("攻撃速度")]
    public float AttackSpeed;
    [Header("「喜」耐性")]
    public float ResistanceFreude;
    [Header("「怒」耐性")]
    public float ResistanceWut;
    [Header("「哀」耐性")]
    public float ResistanceKummer;
    [Header("「楽」耐性")]
    public float ResistanceEinfach;
    [Header("「喜」属性値")]
    public float AttributeFreude;
    [Header("「怒」属性値")]
    public float AttributeWut;
    [Header("「哀」属性値")]
    public float AttributeKummer;
    [Header("「楽」属性値")]
    public float AttributeEinfach;

    /// <summary>
    /// パラメータ取得・更新用インデクサー
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float this[ParamID index]
    {
        get
        {
            switch (index)
            {
                case ParamID.MaxHp:
                    return MaxHp;
                case ParamID.MoveSpeed:
                    return MoveSpeed;
                case ParamID.AttackPower:
                    return AttackPower;
                case ParamID.AttackRange:
                    return AttackRange;
                case ParamID.AttackSpeed:
                    return AttackSpeed;
                case ParamID.ResistanceFreude:
                    return ResistanceFreude;
                case ParamID.ResistanceWut:
                    return ResistanceWut;
                case ParamID.ResistanceKummer:
                    return ResistanceKummer;
                case ParamID.ResistanceEinfach:
                    return ResistanceEinfach;
                case ParamID.AttributeFreude:
                    return AttributeFreude;
                case ParamID.AttributeWut:
                    return AttributeWut;
                case ParamID.AttributeKummer:
                    return AttributeKummer;
                case ParamID.AttributeEinfach:
                    return AttributeEinfach;
                default:
                    return -1;
            }
        }
        set
        {
            switch (index)
            {
                case ParamID.MaxHp:
                    MaxHp = (int)value;
                    return;
                case ParamID.MoveSpeed:
                    MoveSpeed = value;
                    return;
                case ParamID.AttackPower:
                    AttackPower = value;
                    return;
                case ParamID.AttackRange:
                    AttackRange = value;
                    return;
                case ParamID.AttackSpeed:
                    AttackSpeed = value;
                    return;
                case ParamID.ResistanceFreude:
                    ResistanceFreude = value;
                    return;
                case ParamID.ResistanceWut:
                    ResistanceWut = value;
                    return;
                case ParamID.ResistanceKummer:
                    ResistanceKummer = value;
                    return;
                case ParamID.ResistanceEinfach:
                    ResistanceEinfach = value;
                    return;
                case ParamID.AttributeFreude:
                    AttributeFreude = value;
                    return;
                case ParamID.AttributeWut:
                    AttributeWut = value;
                    return;
                case ParamID.AttributeKummer:
                    AttributeKummer = value;
                    return;
                case ParamID.AttributeEinfach:
                    AttributeEinfach = value;
                    return;
                default:
                    return;
            }
        }
    }
}
/// <summary>
/// 武器種
/// </summary>
public enum WeaponType
{
    Knife = 0,
    Lance = 1,
    Mine = 2,
    MachineGun = 3,
    Enemy = 4,
}
/// <summary>
/// 属性種
/// </summary>
public enum AttributeType
{
    /// <summary> "喜" </summary>
    Freude = 0,
    /// <summary> "怒" </summary>
    Wut = 1,
    /// <summary> "哀" </summary>
    Kummer = 2,
    /// <summary> "楽" </summary>
    Einfach = 3,
}
