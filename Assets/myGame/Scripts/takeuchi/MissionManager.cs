using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �~�b�V�����𓝊��Ǘ�����N���X
/// </summary>
public class MissionManager : MonoBehaviour
{
    [SerializeField, Header("�S�~�b�V����")]
    private MissionContainer[] _missionContainers = default;
    [SerializeField]
    private float _waitStartCheckSeconds = 0.5f;
    [SerializeField]
    private float _waitClearCheckSeconds = 0.5f;
    private MissionContainer _currntMission = null;
    private WaitForSeconds _waitStartCheck = default;
    private WaitForSeconds _waitClearCheck = default;
    private bool _isStartMission = false;
    public bool IsGameEnd { get; set; }
    /// <summary>
    /// �~�b�V�����Ǘ����J�n����
    /// </summary>
    /// <param name="targetMission"></param>
    public void StartMission(int targetMission = 0)
    {
        if (_isStartMission || targetMission < 0 || targetMission >= _missionContainers.Length) { return; }
        _currntMission = _missionContainers[targetMission];
        _waitStartCheck = new WaitForSeconds(_waitStartCheckSeconds);
        _waitClearCheck = new WaitForSeconds(_waitClearCheckSeconds);
        _isStartMission = true;
        _currntMission.InitializeMission();
        StartCoroutine(StartMissionExecution());
    }
    /// <summary>
    /// �~�b�V�������s������
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartMissionExecution()
    {
        while (!IsGameEnd)
        {
            _currntMission.CheckMissionStart();
            yield return _waitStartCheck;
            _currntMission.CheckMissionClear();
            yield return _waitClearCheck;
        }
    }
}
