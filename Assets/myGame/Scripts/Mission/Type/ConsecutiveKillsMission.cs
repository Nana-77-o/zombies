using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 連続撃破ミッション
/// </summary>
[CreateAssetMenu(menuName = "Conditions/ConsecutiveKillsMission")]
public class ConsecutiveKillsMission : MissionConditions
{
    [SerializeField, Header("対象ID")]
    private int _targetID = default;
    [SerializeField, Header("非対象ID")]
    private int[] _anTargetIDs = default;
    [SerializeField, Header("目標連続撃破数")]
    private int _targetCount = default;
    private int[] _count = default;
    private bool _check = false;
    private bool _initialize = false;
    public override int TargetCount { get => _targetCount; }
    public override int CurrentCount
    {
        get
        {
            if (GameData.Instance.CountData.ContainsKey(_targetID))
            {
                return 0;
            }
            return GameData.Instance.CountData[_targetID] - _count[0];
        }
    }
    public override bool ClearConditions()
    {
        if (_isClear == true) { return true; }
        InitializeCheck();
        for (int i = 0; i < _anTargetIDs.Length; i++)
        {
            if (_count[i + 1] != GameData.Instance.CountData[_anTargetIDs[i]])
            {
                _count[0] = GameData.Instance.CountData[_targetID];
                _count[i + 1] = GameData.Instance.CountData[_anTargetIDs[i]];
            }
        }
        if (_count[0] + _targetCount <= GameData.Instance.CountData[_targetID])
        {
            _isClear = true;
        }
        return _isClear;
    }
    private void InitializeCheck()
    {
        if (_initialize == false)
        {
            _count = new int[_anTargetIDs.Length + 1];
            _initialize = true;
        }
        if (_check == true) { return; }
        if (GameData.Instance.CountData.ContainsKey(_targetID))
        {
            _count[0] = GameData.Instance.CountData[_targetID];
        }
        else
        {
            GameData.Instance.AddCount(_targetID, 0);
            _count[0] = 0;
        }
        for (int i = 0; i < _anTargetIDs.Length; i++)
        {
            if (GameData.Instance.CountData.ContainsKey(_anTargetIDs[i]) == false)
            {
                GameData.Instance.AddCount(_anTargetIDs[i], 0);
                _count[i + 1] = 0;
            }
            else
            {
                _count[i + 1] = GameData.Instance.CountData[_anTargetIDs[i]];
            }
        }
        _check = true;
    }
    public override void InitializeConditions()
    {
        _count = null;
        _initialize = false;
        _check = false;
        _isClear = false;
    }
}
