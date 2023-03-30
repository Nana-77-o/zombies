using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �J�E���g��������C����
[CreateAssetMenu(menuName = "MissionCount/Count")]
/// </summary>
public class MissionAddCount : ScriptableObject
{
    /// <summary> �S���j�J�E���gID </summary>
    public const int DEFAULT_ADD_ID = 0;
    [SerializeField,Header("�������Ƃ̃J�E���gID")]
    private AttributeAddID _attributeIDs = default;
    [SerializeField, Header("����킲�Ƃ̃J�E���gID")]
    private WeaponAddID _weaponIDs = default;
    [SerializeField, Header("�G�p�m�[�_���[�W����pID")]
    private int _enemyNoDamageID = default;
    [SerializeField, Header("�������j����pID")]
    private int _simultaneousID = default;
    [SerializeField]
    private GameObject[] _attributeEffects = default;
    /// <summary>
    /// �_���[�W���󂯂����̃J�E���g
    /// </summary>
    /// <param name="damage"></param>
    public void AddDamageCount(Damage damage)
    {
        GameData.Instance.AddCount(_enemyNoDamageID);
    }
    /// <summary>
    /// ���j���̃J�E���g
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="id"></param>
    public void AddCount(Damage damage, int id)
    {
        if (damage != null)
        {
            AddAttributeCount(damage.Attribute);
            AddWeaponCount(damage.Type);
        }
        if (id != DEFAULT_ADD_ID)
        {
            GameData.Instance.AddCount(id);
        }
        GameData.Instance.AddCount(DEFAULT_ADD_ID);
        GameData.Instance.AddCount(_simultaneousID);
    }
    public GameObject GetAttributeEffect(AttributeType attribute)
    {
        switch (attribute)
        {
            case AttributeType.Freude:
                return _attributeEffects[0];
            case AttributeType.Wut:
                return _attributeEffects[1];
            case AttributeType.Kummer:
                return _attributeEffects[2];
            case AttributeType.Einfach:
                return _attributeEffects[3];
            default:
                break;
        }
        return null;
    }
    private void AddAttributeCount(AttributeType attribute)
    {
        switch (attribute)
        {
            case AttributeType.Freude:
                GameData.Instance.AddCount(_attributeIDs.FreudeId);
                break;
            case AttributeType.Wut:
                GameData.Instance.AddCount(_attributeIDs.WutId);
                break;
            case AttributeType.Kummer:
                GameData.Instance.AddCount(_attributeIDs.KummerId);
                break;
            case AttributeType.Einfach:
                GameData.Instance.AddCount(_attributeIDs.EinfachId);
                break;
            default:
                break;
        }
    }
    private void AddWeaponCount(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Knife:
                if (_weaponIDs.KnifeId < 0)
                {
                    return;
                }
                GameData.Instance.AddCount(_weaponIDs.KnifeId);
                break;
            case WeaponType.Lance:
                if (_weaponIDs.LanceId < 0)
                {
                    return;
                }
                GameData.Instance.AddCount(_weaponIDs.LanceId);
                break;
            case WeaponType.MachineGun:
                if (_weaponIDs.MachineGunId < 0)
                {
                    return;
                }
                GameData.Instance.AddCount(_weaponIDs.MachineGunId);
                break;
            case WeaponType.Mine:
                if (_weaponIDs.MineId < 0)
                {
                    return;
                }
                GameData.Instance.AddCount(_weaponIDs.MineId);
                break;
            case WeaponType.Enemy:
                break;
            default:
                break;
        }
    }
}
[Serializable]
public struct AttributeAddID
{
    [Header("�쑮���Ō��j�J�E���g�𑝂₷ID")]
    public int FreudeId; 
    [Header("�{�����Ō��j�J�E���g�𑝂₷ID")]
    public int WutId;
    [Header("�������Ō��j�J�E���g�𑝂₷ID")]
    public int KummerId;
    [Header("�y�����Ō��j�J�E���g�𑝂₷ID")]
    public int EinfachId;
}
[Serializable]
public struct WeaponAddID
{
    [Header("�i�C�t�Ō��j�J�E���g�𑝂₷ID")]
    public int KnifeId;
    [Header("���Ō��j�J�E���g�𑝂₷ID")]
    public int LanceId;
    [Header("�e��Ō��j�J�E���g�𑝂₷ID")]
    public int MachineGunId;
    [Header("���e�Ō��j�J�E���g�𑝂₷ID")]
    public int MineId;
}