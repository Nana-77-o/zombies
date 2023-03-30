using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 墓石出現アクション
/// </summary>
[CreateAssetMenu(menuName = "MissionStart/TouchObjPop")]
public class TouchObjPopAction : MissionStartAction
{
    [SerializeField]
    private TouchObject _popObj = default;
    [SerializeField]
    private int _popCount = 10;
    public override void StartAction()
    {
        BossMissionManager.Instance.PopObj(_popObj.gameObject, _popCount);
    }
}
