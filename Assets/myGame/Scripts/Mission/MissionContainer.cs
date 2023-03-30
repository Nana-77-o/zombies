using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 複数のミッションをまとめるクラス
/// </summary>
[CreateAssetMenu(menuName ="MissionContainer")]
public class MissionContainer : ScriptableObject
{
    [SerializeField,Header("全ミッション")]
    private MissionBase[] _allMissions = default;
    [SerializeField, Header("全コンテナ")]
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
    /// 所持するミッションの開始可能なものを全て開始する
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
