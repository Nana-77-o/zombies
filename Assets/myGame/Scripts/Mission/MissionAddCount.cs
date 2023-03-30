using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// カウント処理を一任する
[CreateAssetMenu(menuName = "MissionCount/Count")]
/// </summary>
public class MissionAddCount : ScriptableObject
{
    /// <summary> 全撃破カウントID </summary>
    public const int DEFAULT_ADD_ID = 0;
    [SerializeField,Header("属性ごとのカウントID")]
    private AttributeAddID _attributeIDs = default;
    [SerializeField, Header("武器種ごとのカウントID")]
    private WeaponAddID _weaponIDs = default;
    [SerializeField, Header("敵用ノーダメージ判定用ID")]
    private int _enemyNoDamageID = default;
    [SerializeField, Header("同時撃破判定用ID")]
    private int _simultaneousID = default;
    [SerializeField]
    private GameObject[] _attributeEffects = default;
    /// <summary>
    /// ダメージを受けた時のカウント
    /// </summary>
    /// <param name="damage"></param>
    public void AddDamageCount(Damage damage)
    {
        GameData.Instance.AddCount(_enemyNoDamageID);
    }
    /// <summary>
    /// 撃破時のカウント
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
    [Header("喜属性で撃破カウントを増やすID")]
    public int FreudeId; 
    [Header("怒属性で撃破カウントを増やすID")]
    public int WutId;
    [Header("哀属性で撃破カウントを増やすID")]
    public int KummerId;
    [Header("楽属性で撃破カウントを増やすID")]
    public int EinfachId;
}
[Serializable]
public struct WeaponAddID
{
    [Header("ナイフで撃破カウントを増やすID")]
    public int KnifeId;
    [Header("槍で撃破カウントを増やすID")]
    public int LanceId;
    [Header("銃器で撃破カウントを増やすID")]
    public int MachineGunId;
    [Header("爆弾で撃破カウントを増やすID")]
    public int MineId;
}