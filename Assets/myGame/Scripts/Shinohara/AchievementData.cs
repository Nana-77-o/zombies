using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AchievementData : ScriptableObject
{
    [SerializeField, Header("�̍��f�[�^")]
    List<Achievement> _achievementInfos = default;
    [SerializeField, Header("�������Ă��Ȃ����̕���")]
    string _conditionsText = default;

    public List<Achievement> AchievementInfos { get => _achievementInfos; }
}

/// <summary>�R���{���ɂ���ĕ\������C���X�g</summary>
[Serializable]
public class Achievement
{
    [SerializeField, Header("ID")]
    public int id;

    [SerializeField, Header("�̍���")]
    public string _label;

    [SerializeField, Header("�ڍ� 1�s��")]
    public string _detail1;

    [SerializeField, Header("�ڍ� 2�s��")]
    public string _detail2;

    [SerializeField, Header("�������")]
    public string _conditions;
}
