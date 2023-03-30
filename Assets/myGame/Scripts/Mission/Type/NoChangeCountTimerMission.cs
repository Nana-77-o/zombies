using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一定時間変動無しミッション
/// </summary>
[CreateAssetMenu(menuName = "Conditions/NoChangeCountTimer")]
public class NoChangeCountTimerMission : MissionConditions
{
    [SerializeField, Header("対象ID")]
    private int _targetID = default;
    [SerializeField, Header("変動無し目標時間")]
    private float _targetTime = default;
    private int _targetCount = default;
    private bool _initialize = false;
    private float _timer = -1f;
    public override bool ClearConditions()
    {
        if (_isClear == true) { return true; }
        if (_initialize == false)
        {
            if (GameData.Instance.CountData.ContainsKey(_targetID))
            {
                _targetCount = GameData.Instance.CountData[_targetID];
            }
            else
            {
                GameData.Instance.AddCount(_targetID, 0);
                _targetCount = 0;
            }
            _timer = GameData.Instance.GameTime;
            _initialize = true;
            return _isClear;
        }
        if (_targetCount != GameData.Instance.CountData[_targetID])
        {
            _targetCount = GameData.Instance.CountData[_targetID];
            _timer = GameData.Instance.GameTime;
            return _isClear;
        }
        if (GameData.Instance.GameTime - _timer >= _targetTime)
        {
            _isClear = true;
        }
        return _isClear;
    }
    public override void InitializeConditions()
    {
        _targetCount = default;
        _initialize = false;
        _timer = -1f;
        _isClear = false;
    }
}
