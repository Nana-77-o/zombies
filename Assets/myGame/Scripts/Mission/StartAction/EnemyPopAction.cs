using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy�|�b�v�A�N�V����
/// </summary>
[CreateAssetMenu(menuName = "MissionStart/EnemyPop")]
public class EnemyPopAction : MissionStartAction
{
    [SerializeField]
    private GameObject _popEnemy = default;
    [SerializeField]
    private int _popCount = 10;
    public override void StartAction()
    {
        BossMissionManager.Instance.PopMissionEnemys(_popEnemy, _popCount);
    }
}
