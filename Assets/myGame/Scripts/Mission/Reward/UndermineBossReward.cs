using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Boss��̉�
/// </summary>
[CreateAssetMenu(menuName = "Reward/UndermineBossReward")]
public class UndermineBossReward : MissionReward
{
    [SerializeField]
    private int _targetId = default;
    public override void GiveAReward()
    {
        if(BossMissionManager.Instance != null)
        {
            BossMissionManager.Instance.UndermineBoss(_targetId);
        }
        Debug.Log($"Boss:{_targetId}��̉�");
    }
}
