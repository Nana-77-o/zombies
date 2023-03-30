using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MissionSelector")]
public class MissionSelector : ScriptableObject
{
    [SerializeField]
    private MissionBase[] _missions = default;
    [SerializeField]
    private int _selectCount = 1;
    public int MissionCount { get => _selectCount; }
    public IEnumerable<MissionBase> GetMissions()
    {
        for (int i = 0; i < _selectCount; i++)
        {
            yield return _missions[i];
        }
    }
    public void ShuffleMission()
    {
        for (int i = 0; i < _missions.Length; i++)
        {
            var r = Random.Range(0, _missions.Length);
            var point = _missions[i];
            _missions[i] = _missions[r];
            _missions[r] = point;
        }
    }
}
