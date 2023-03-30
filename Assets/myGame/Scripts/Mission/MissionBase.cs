using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mission���N���X
/// </summary>
[CreateAssetMenu(menuName = "Mission/BaseMission")]
public class MissionBase : ScriptableObject
{
    [SerializeField, Header("�~�b�V�������e")]
    protected string _missionName = "";
    [SerializeField, Header("�~�b�V�����̏d�v�x")]
    protected int _missionImportance = 0;
    [SerializeField, Header("�~�b�V�����̊J�n����")]
    protected MissionConditions[] _startConditions = null;
    [SerializeField, Header("�~�b�V�����J�n���̃C�x���g")]
    protected MissionStartAction[] _startActions = null;
    [SerializeField, Header("�~�b�V�����̃N���A����")]
    protected MissionConditions[] _clearConditions = null;
    [SerializeField, Header("�~�b�V�����̕�V")]
    protected MissionReward[] _rewards = null;
    protected MissionReward _additionalReward = null;
    protected bool _isCanBeStarted = default;
    /// <summary> �~�b�V�����̓��e </summary>
    public string MissionName { get => _missionName; }
    /// <summary> �~�b�V�����̃N���A�ڕW�� </summary>
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
    /// <summary> �~�b�V�����̐i�s�x </summary>
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
    /// �~�b�V�����N���A�t���O
    /// </summary>
    public bool IsClearMission
    {
        get => IsEndMission;
    }
    /// <summary> �J�n�\�t���O </summary>
    public virtual bool IsCanBeStarted
    {
        get
        {
            if (IsEndMission || IsMissionExecution) { return false; }
            if (_isCanBeStarted == true) { return true; }
            //�X�^�[�g������S�Ė������Ă��邩�m�F����
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
    /// <summary> ���s���t���O </summary>
    public bool IsMissionExecution { get; protected set; }
    /// <summary> �I���t���O </summary>
    public bool IsEndMission { get; protected set; }
    /// <summary> �d�v�x </summary>
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
    /// �~�b�V�������J�n����
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
    /// �N���A�����𖞂����Ă��邩�Ԃ�
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
    /// ��V��^����
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
    /// �~�b�V�������s�����A�N���A�����𖞂����ƕ�V��^����
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
