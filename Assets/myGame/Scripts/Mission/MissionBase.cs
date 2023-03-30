using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mission基底クラス
/// </summary>
[CreateAssetMenu(menuName = "Mission/BaseMission")]
public class MissionBase : ScriptableObject
{
    [SerializeField, Header("ミッション内容")]
    protected string _missionName = "";
    [SerializeField, Header("ミッションの重要度")]
    protected int _missionImportance = 0;
    [SerializeField, Header("ミッションの開始条件")]
    protected MissionConditions[] _startConditions = null;
    [SerializeField, Header("ミッション開始時のイベント")]
    protected MissionStartAction[] _startActions = null;
    [SerializeField, Header("ミッションのクリア条件")]
    protected MissionConditions[] _clearConditions = null;
    [SerializeField, Header("ミッションの報酬")]
    protected MissionReward[] _rewards = null;
    protected MissionReward _additionalReward = null;
    protected bool _isCanBeStarted = default;
    /// <summary> ミッションの内容 </summary>
    public string MissionName { get => _missionName; }
    /// <summary> ミッションのクリア目標数 </summary>
    public int TargetCount
    {
        get
        {
            int count = 0;
            foreach (var condition in _clearConditions)
            {
                count += condition.TargetCount;
            }
            return count;
        }
    }
    /// <summary> ミッションの進行度 </summary>
    public int CurrentCount
    {
        get
        {
            int count = 0;
            foreach (var condition in _clearConditions)
            {
                count += condition.CurrentCount;
            }
            return count;
        }
    }
    /// <summary>
    /// ミッションクリアフラグ
    /// </summary>
    public bool IsClearMission
    {
        get => IsEndMission;
    }
    /// <summary> 開始可能フラグ </summary>
    public virtual bool IsCanBeStarted
    {
        get
        {
            if (IsEndMission || IsMissionExecution) { return false; }
            if (_isCanBeStarted == true) { return true; }
            //スタート条件を全て満たしているか確認する
            foreach (var condition in _startConditions)
            {
                if (!condition.ClearConditions())
                {
                    return false;
                }
            }
            _isCanBeStarted = true;
            return true;
        }
    }
    /// <summary> 実行中フラグ </summary>
    public bool IsMissionExecution { get; protected set; }
    /// <summary> 終了フラグ </summary>
    public bool IsEndMission { get; protected set; }
    /// <summary> 重要度 </summary>
    public int Importance { get => _missionImportance; }
    public virtual void InitializeMission()
    {
        IsMissionExecution = false;
        IsEndMission = false;
        _isCanBeStarted = false;
        foreach (var condition in _clearConditions)
        {
            condition.InitializeConditions();
        }
        foreach (var condition in _startConditions)
        {
            condition.InitializeConditions();
        }
    }
    /// <summary>
    /// ミッションを開始する
    /// </summary>
    public void StartMission()
    {
        if (!_isCanBeStarted || IsEndMission || IsMissionExecution)
        {
            return;
        }
        foreach (var action in _startActions)
        {
            action.StartAction();
        }
        IsMissionExecution = true;
        CheckMissionClear();
    }
    /// <summary>
    /// クリア条件を満たしているか返す
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckClearConditions()
    {
        foreach (var condition in _clearConditions)
        {
            if (condition.ClearConditions() == false)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 報酬を与える
    /// </summary>
    protected void GiveARewards()
    {
        foreach (var reward in _rewards)
        {
            reward.GiveAReward();
        }
        if (_additionalReward != null)
        {
            _additionalReward.GiveAReward();
        }
    }
    /// <summary>
    /// ミッション実行処理、クリア条件を満たすと報酬を与える
    /// </summary>
    /// <returns></returns>
    public virtual void CheckMissionClear()
    {
        if (!CheckClearConditions()) { return; }
        GiveARewards();
        IsMissionExecution = false;
        IsEndMission = true;
    }
    public void SetReward(MissionReward reward)
    {
        _additionalReward = reward;
    }
}
