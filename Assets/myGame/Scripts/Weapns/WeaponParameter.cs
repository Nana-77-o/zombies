using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="WeaponParameter")]
public class WeaponParameter : ScriptableObject
{
    [SerializeField, Header("���햼��")]
    protected string _weaponName = "";
    [SerializeField, Header("�����")]
    protected WeaponType _weapnType = default;
    [SerializeField, Header("����")]
    protected AttributeType _attributeType = default;
    [SerializeField, Header("�_���[�W")]
    protected int _weaponDamage = 1;
    [SerializeField, Header("�����l")]
    protected float _attributePower = 1f;
    [SerializeField, Header("�ő�U����")]
    protected int _maxCount = 5;
    [SerializeField, Header("�`���[�W����")]
    protected float _chargeTime = 5f;
    protected int _addCount = 0;
    /// <summary> ���햼�� </summary>
    public string Name { get => _weaponName; }
    /// <summary> ������ </summary>
    public WeaponType WeaponType { get => _weapnType; }
    /// <summary> ������� </summary>
    public AttributeType AttributeType { get => _attributeType; }
    /// <summary> �_���[�W�� </summary>
    public int Damage { get => _weaponDamage; }
    /// <summary> �����l </summary>
    public float AttributePower { get => _attributePower; }
    public int MaxCount
    {
        get => _maxCount + _addCount;
    }
    public int AddCount
    {
        get => _addCount;

        set
        {
            _addCount = value;
        }
    }
    public float ChargeTime { get => _chargeTime; }
}
