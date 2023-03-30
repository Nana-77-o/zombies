using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Conditions/CountMission")]
public class CountMission : MissionConditions
{
    [SerializeField,Header("�Ώ�ID")]
    private int _targetID = default;
    [SerializeField,Header("�ڕW��")]
    private int _targetCount = default;
    public override int TargetCount { get => _targetCount; }
    public override int CurrentCount 
    {
        get 
        {
            if (!GameData.Instance.CountData.ContainsKey(_targetID))
            {
                return 0;
            }
            return GameData.Instance.CountData[_targetID]; 
        }
    }
    public override bool ClearConditions()
    {
        if (_isClear == true) { return true; }
        _isClear = GameData.Instance.CountData.ContainsKey(_targetID) && GameData.Instance.CountData[_targetID] >= _targetCount;
        return _isClear;
    }
    public override void InitializeConditions()
    {
        _isClear = false;
    }
}
