using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �L�����N�^�[�̃p�����[�^�N���X
/// </summary>
public class CharacterParameter
{
    /// <summary> ����퐔 </summary>
    public static readonly int WEAPON_TYPES = Enum.GetValues(typeof(WeaponType)).Length;
    /// <summary> �����퐔 </summary>
    public static readonly int ATTRIBUTE_TYPES = Enum.GetValues(typeof(AttributeType)).Length;
    #region Private Field
    /// <summary> �ő�ϋv�l </summary>
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

    /// <summary> �ϋv�l </summary>
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
    /// <summary> ��b�U���� </summary>
    public float DefaultPower
    {
        get => _power;
        set
        {
            _power = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> �ړ����x </summary>
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            _moveSpeed = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> �U���͈� </summary>
    public float AttackRange
    {
        get => _attackRange;
        set
        {
            _attackRange = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> �U�����x </summary>
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            _attackSpeed = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> �U�������l </summary>
    public float[] AttributePower
    {
        get => _attributePower;
        set
        {
            _attributePower = value;
            DelUpdateParameter?.Invoke();
        }
    }
    /// <summary> �����ϐ��l </summary>
    public float[] Resistances
    {
        get => _resistances;
        set
        {
            _resistances = value;
            DelUpdateParameter?.Invoke();
        }
    }

    /// <summary> Parameter�X�V���̃C�x���g </summary>
    public event Action DelUpdateParameter = default;
    /// <summary> ���S���ɌĂ΂��C�x���g </summary>
    public event Action DelDead = default;
    /// <summary>
    /// �p�����[�^�R���X�g���N�^
    /// </summary>
    /// <param name="moveSpeed">�ړ����x</param>
    /// <param name="maxHp">�ő�HP</param>
    /// <param name="attackRange">�ő�`���[�W��</param>
    /// <param name="attackSpeed">�`���[�W���x</param>
    /// <param name="resistanceFreude">"��"�ϐ�</param>
    /// <param name="resistanceWut">"�{"�ϐ�</param>
    /// <param name="resistanceKummer">"��"�ϐ�</param>
    /// <param name="resistanceEinfach">"�y"�ϐ�</param>
    /// <param name="attributeFreude">"��"�����l</param>
    /// <param name="attributeWut">"�{"�����l</param>
    /// <param name="attributeKummer">"��"�����l</param>
    /// <param name="attributeEinfach">"�y"�����l</param>
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
    /// �p�����[�^�擾�E�X�V�p�C���f�N�T�[
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
    [Header("�ړ����x")]
    public float MoveSpeed;
    [Header("�ő�HP")]
    public int MaxHp;
    [Header("�U����")]
    public float AttackPower;
    [Header("�U���͈�")]
    public float AttackRange;
    [Header("�U�����x")]
    public float AttackSpeed;
    [Header("�u��v�ϐ�")]
    public float ResistanceFreude;
    [Header("�u�{�v�ϐ�")]
    public float ResistanceWut;
    [Header("�u���v�ϐ�")]
    public float ResistanceKummer;
    [Header("�u�y�v�ϐ�")]
    public float ResistanceEinfach;
    [Header("�u��v�����l")]
    public float AttributeFreude;
    [Header("�u�{�v�����l")]
    public float AttributeWut;
    [Header("�u���v�����l")]
    public float AttributeKummer;
    [Header("�u�y�v�����l")]
    public float AttributeEinfach;

    /// <summary>
    /// �p�����[�^�擾�E�X�V�p�C���f�N�T�[
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
/// �����
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
/// ������
/// </summary>
public enum AttributeType
{
    /// <summary> "��" </summary>
    Freude = 0,
    /// <summary> "�{" </summary>
    Wut = 1,
    /// <summary> "��" </summary>
    Kummer = 2,
    /// <summary> "�y" </summary>
    Einfach = 3,
}
