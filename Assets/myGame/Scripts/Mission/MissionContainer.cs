using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����̃~�b�V�������܂Ƃ߂�N���X
/// </summary>
[CreateAssetMenu(menuName ="MissionContainer")]
public class MissionContainer : ScriptableObject
{
    [SerializeField,Header("�S�~�b�V����")]
    private MissionBase[] _allMissions = default;
    [SerializeField, Header("�S�R���e�i")]
    private MissionContainer[] _containers = default;
    public void InitializeMission()
    {
        foreach (var mission in _allMissions)
        {
            mission.InitializeMission();
        }
        foreach (var container in _containers)
        {
            container.InitializeMission();
        }
    }
    /// <summary>
    /// ��������~�b�V�����̊J�n�\�Ȃ��̂�S�ĊJ�n����
    /// </summary>
    public void CheckMissionStart()
    {
        foreach (var mission in _allMissions)
        {
            if (mission == null || mission.IsCanBeStarted == false) { continue; }
            mission.StartMission();
        }
        foreach (var container in _containers)
        {
            if (container == null) { continue; }
            container.CheckMissionStart();
        }
    }
    public void CheckMissionClear()
    {
        foreach (var mission in _allMissions)
        {
            if (mission == null || mission.IsMissionExecution == false) { continue; }
            mission.CheckMissionClear();
        }
        foreach (var container in _containers)
        {
            if (container == null) { continue; }
            container.CheckMissionClear();
        }
    }
}
