using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// MissionŠî’êƒNƒ‰ƒX
/// </summary>
[CreateAssetMenu(menuName = "Mission/Endless")]
public class EndlessMission : MissionBase
{
    private List<IEndlessMission> _startEndlessConditions = new List<IEndlessMission>();
    private List<IEndlessMission> _clearEndlessConditions = new List<IEndlessMission>();
    public override bool IsCanBeStarted
    {
        get
        {
            if (IsEndMission || IsMissionExecution) { return false; }
            if (_isCanBeStarted == true) { return true; }
            SetConditions();
            _isCanBeStarted = true;
            return true;
        }
    }
    private void SetConditions()
    {
        _startEndlessConditions.Clear();
        _clearEndlessConditions.Clear();
        foreach (var conditions in _startConditions)
        {
            var endless = (IEndlessMission)conditions;
            if (endless != null)
            {
                _startEndlessConditions.Add(endless);
            }
        }
        foreach (var conditions in _clearConditions)
        {
            var endless = (IEndlessMission)conditions;
            if (endless != null)
            {
                _clearEndlessConditions.Add(endless);
            }
        }
    }
    public override void CheckMissionClear()
    {
        if (CheckClearConditions() == false) { return; }
        GiveARewards();
        foreach (var conditions in _startEndlessConditions)
        {
            conditions.UpdateConditions();
        }
        foreach (var conditions in _clearEndlessConditions)
        {
            conditions.UpdateConditions();
        }
    }
}
public interface IEndlessMission
{
    void UpdateConditions();
}
