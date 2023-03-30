using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 属性レベルアップ
/// </summary>
[CreateAssetMenu(menuName = "Reward/LevelUpReward")]
public class LevelUpReward : MissionReward 
{
    [SerializeField]
    private AttributeType _levelUpAttribute = default;
    [SerializeField]
    private float _addParam = 0.1f;
    public override void GiveAReward()
    {
        if(BossMissionManager.Instance != null)
        {
            BossMissionManager.Instance.Player.GetParameter().AttributePower[(int)_levelUpAttribute] += _addParam;
        }
    }
}
