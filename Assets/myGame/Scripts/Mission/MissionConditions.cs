using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �~�b�V�����̏����ݒ���N���X
/// </summary>
public abstract class MissionConditions : ScriptableObject
{
    protected bool _isClear = false;
    public virtual int TargetCount { get; protected set; }
    public virtual int CurrentCount { get; protected set; }
    /// <summary>
    /// �����𖞂����Ă���ꍇ��true
    /// </summary>
    /// <returns></returns>
    public abstract bool ClearConditions();
    public abstract void InitializeConditions();
}
