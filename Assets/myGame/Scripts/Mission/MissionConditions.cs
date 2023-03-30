using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ミッションの条件設定基底クラス
/// </summary>
public abstract class MissionConditions : ScriptableObject
{
    protected bool _isClear = false;
    public virtual int TargetCount { get; protected set; }
    public virtual int CurrentCount { get; protected set; }
    /// <summary>
    /// 条件を満たしている場合はtrue
    /// </summary>
    /// <returns></returns>
    public abstract bool ClearConditions();
    public abstract void InitializeConditions();
}
