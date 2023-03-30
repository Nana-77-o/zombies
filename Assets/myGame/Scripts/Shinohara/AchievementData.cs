using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AchievementData : ScriptableObject
{
    [SerializeField, Header("称号データ")]
    List<Achievement> _achievementInfos = default;
    [SerializeField, Header("解放されていない時の文言")]
    string _conditionsText = default;

    public List<Achievement> AchievementInfos { get => _achievementInfos; }
}

/// <summary>コンボ数によって表示するイラスト</summary>
[Serializable]
public class Achievement
{
    [SerializeField, Header("ID")]
    public int id;

    [SerializeField, Header("称号名")]
    public string _label;

    [SerializeField, Header("詳細 1行目")]
    public string _detail1;

    [SerializeField, Header("詳細 2行目")]
    public string _detail2;

    [SerializeField, Header("解放条件")]
    public string _conditions;
}
