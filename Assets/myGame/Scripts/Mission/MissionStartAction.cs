using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ミッションの開始時基底クラス
/// </summary>
public abstract class MissionStartAction : ScriptableObject
{
    /// <summary>
    /// 開始時に行うこと
    /// </summary>
    public abstract void StartAction();
}