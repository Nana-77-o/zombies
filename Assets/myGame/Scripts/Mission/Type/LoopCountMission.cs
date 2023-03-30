using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Endless/CountMissionLoop")]
public class LoopCountMission : MissionConditions, IEndlessMission
{
    [SerializeField, Header("‘ÎÛID")]
    private int _targetID = default;
    [SerializeField, Header("–Ú•W”")]
    private int _targetCount = default;
    private int _currentTargetcount = default;
    public override int TargetCount { get => _currentTargetcount; }
    public override int CurrentCount
    {
        get
        {
            if (GameData.Instance.CountData.ContainsKey(_targetID) == false)
            {
                return 0;
            }
            return GameData.Instance.CountData[_targetID];
        }
    }
    public override bool ClearConditions()
    {
        if (_isClear == true) { return true; }
        _isClear = GameData.Instance.CountData.ContainsKey(_targetID) && GameData.Instance.CountData[_targetID] >= _currentTargetcount;
        return _isClear;
    }
    public override void InitializeConditions()
    {
        _isClear = false;
        _currentTargetcount = _targetCount;
    }
    public void UpdateConditions()
    {
        _currentTargetcount += _targetCount;
        _isClear = false;
    }
}
