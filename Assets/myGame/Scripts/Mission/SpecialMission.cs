using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialMission : MonoBehaviour
{
    public const int SPECIAL_MISSION_COUNT = 10;
    [SerializeField]
    private MissionReward[] _rewardList = default;
    [SerializeField]
    private MissionBase[] _specialMissions = default;
    [SerializeField]
    private MissionSelector[] _missionSelectorList = default;
    [SerializeField]
    private float _waitStartCheckSeconds = 0.5f;
    [SerializeField]
    private float _waitClearCheckSeconds = 0.5f;
    [SerializeField]
    private Text _currentMission = default;
    private WaitForSeconds _waitStartCheck = default;
    private WaitForSeconds _waitClearCheck = default;
    private bool _isStartMission = false;
    public bool IsGameEnd { get; set; }
    /// <summary>
    /// クリアしたミッションの数
    /// </summary>
    public int ClearMissionCount
    {
        get
        {
            int count = 0;
            foreach(MissionBase mission in _specialMissions)
            {
                if (mission.IsClearMission == true)
                {
                    count++;
                }
            }
            return count;
        }
    }
    /// <summary>
    /// ミッションの数
    /// </summary>
    public int MissionCount { get { return _specialMissions.Length; } }
    public void InitializedSpecialMission()
    {
        if (_missionSelectorList != null)
        {
            SetMission();
        }
        for (int i = 0; i < SPECIAL_MISSION_COUNT; i++)
        {
            var r = Random.Range(0, SPECIAL_MISSION_COUNT);
            var mission = _specialMissions[i];
            _specialMissions[i] = _specialMissions[r];
            _specialMissions[r] = mission;
        }
        for (int i = 0; i < BossMissionManager.Instance.BossCount; i++)
        {
            _specialMissions[i].SetReward(_rewardList[i]);
        }
        InitializeMission();
    }
    private void SetMission()
    {
        _specialMissions = new MissionBase[SPECIAL_MISSION_COUNT];
        int index = 0;
        foreach(var selector in _missionSelectorList)
        {
            selector.ShuffleMission();
            foreach (var mission in selector.GetMissions())
            {
                _specialMissions[index] = mission;
                index++;
                if (index >= SPECIAL_MISSION_COUNT)
                {
                    return;
                }
            }
        }
    }
    /// <summary>
    /// ミッション管理を開始する
    /// </summary>
    public void StartMission()
    {
        if (_isStartMission == true)
        {
            return;
        }
        _waitStartCheck = new WaitForSeconds(_waitStartCheckSeconds);
        _waitClearCheck = new WaitForSeconds(_waitClearCheckSeconds);
        _isStartMission = true;
        StartCoroutine(StartMissionExecution());
    }
    /// <summary>
    /// ミッション実行中処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartMissionExecution()
    {
        while (!IsGameEnd)
        {
            CheckMissionStart();
            yield return _waitStartCheck;
            CheckMissionClear();
            yield return _waitClearCheck;
            ShowMission();
        }
    }
    private void InitializeMission()
    {
        foreach (var mission in _specialMissions)
        {
            mission.InitializeMission();
        }
    }
    private void CheckMissionStart()
    {
        foreach (var mission in _specialMissions)
        {
            if (mission == null || !mission.IsCanBeStarted) { continue; }
            mission.StartMission();
        }
    }
    private void CheckMissionClear()
    {
        foreach (var mission in _specialMissions)
        {
            if (mission == null || !mission.IsMissionExecution) { continue; }
            mission.CheckMissionClear();
        }
    }
    private void ShowMission()
    {
        if (_currentMission == null) { return; }
        int importance = -1;
        _currentMission.text = "";
        foreach (var mission in _specialMissions)
        {
            if (importance < mission.Importance && mission.IsEndMission == false)
            {
                _currentMission.text = mission.MissionName;
                importance = mission.Importance;
            }
        }
    }
    public IEnumerable<MissionBase> MissionData()
    {
        foreach(var mission in _specialMissions)
        {
            yield return mission;
        }
    }
}
