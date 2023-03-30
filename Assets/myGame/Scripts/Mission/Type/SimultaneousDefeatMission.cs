using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// “¯ŽžŒ‚”jƒ~ƒbƒVƒ‡ƒ“
/// </summary>
[CreateAssetMenu(menuName = "Conditions/SimultaneousDefeatMission")]
public class SimultaneousDefeatMission : MissionConditions
{
    [SerializeField, Header("‘ÎÛID")]
    private int _targetID = default;
    [SerializeField, Header("“¯Žž–Ú•WŒ‚”j”")]
    private int _targetCount = default;
    private int _count = 0;
    private bool _check = false;
    public override int TargetCount { get => _targetCount; }
    public override int CurrentCount
    {
        get
        {
            if (GameData.Instance.CountData.ContainsKey(_targetID))
            {
                return 0;
            }
            return GameData.Instance.CountData[_targetID] - _count;
        }
    }
    public override bool ClearConditions()
    {
        if (_isClear == true) { return true; }
        if (_check == false)
        {
            if (GameData.Instance.CountData.ContainsKey(_targetID))
            {
                _count += GameData.Instance.CountData[_targetID];
            }
            else
            {
                GameData.Instance.AddCount(_targetID, 0);
                _count = 0;
            }
            _check = true;
            return _isClear;
        }
        else
        {
            if (_count + _targetCount <= GameData.Instance.CountData[_targetID])
            {
                _isClear = true;
            }
            else
            {
                _count = 0;
                _count += GameData.Instance.CountData[_targetID];
            }
        }
        return _isClear;
    }

    public override void InitializeConditions()
    {
        _count = 0;
        _check = false;
        _isClear= false;
    }
}
