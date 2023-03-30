using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルトに使用する可変のデータ群
/// </summary>
public class ResultVariableData : MonoBehaviour
{
    private ResultImmutableData _immutableData = null;

    [Tooltip("インスペクタで値の変化を追跡する用"), SerializeField]
    private int _previousTotalPoint = 0;
    [Tooltip("インスペクタで値の変化を追跡する用"), SerializeField]
    private int _additionalPoint = 0;
    [Tooltip("インスペクタで値の変化を追跡する用"), SerializeField]
    private int _latestTotalPoint = 0;
    [SerializeField]
    private TestKillScore[] _testKillScores = default;

    /// <summary> PlayerPrefsへ保存されているトータルスコアへのキー </summary>
    private string _totalScoreKey = "TotalScore";

    private readonly int _killScoreID = 0;
    private readonly int _helpScoreID = 12;

    /// <summary>
    /// 加算前のポイント
    /// </summary>
    public int PreviousTotalPoint => _previousTotalPoint;
    /// <summary>
    /// 加算分のポイント
    /// </summary>
    public int AdditionalPoint => _additionalPoint;
    /// <summary>
    /// 加算後のポイント
    /// </summary>
    public int LatestTotalPoint => _latestTotalPoint;
    /// <summary>
    /// 
    /// </summary>
    public TestKillScore[] TestKillScores => _testKillScores;


    /// <summary>
    /// テスト用 : IDと討伐数の組み合わせ
    /// </summary>
    public struct TestKillScore
    {
        public int _id;
        public int _killScore;
    }
    private void Start()
    {
        _immutableData = GetComponent<ResultImmutableData>();
    }

    /// <summary> 加算前ポイントをセットする </summary>
    public void SetPreviousTotalPoint(int value)
    {
        _previousTotalPoint = value;
    }
    /// <summary> 加算ポイントをセットする </summary>
    public void SetAdditionalPoint(int value)
    {
        _additionalPoint = value;
    }
    /// <summary> 加算後のポイントをセットする </summary>
    public void SetLatestTotalPoint(int value)
    {
        _latestTotalPoint = value;
    }
    public void UpdateValue()
    {
        // PlayerPrefsに保存されている これまでのポイントを取得してくる
        _previousTotalPoint = PlayerPrefs.GetInt(_totalScoreKey, 0);
        // 討伐数や救出数から加算ポイントを取得する
        var killIndex = _immutableData.GetRankIndex(_immutableData.GetScoreValue(ScoreType.Kill, _killScoreID), ScoreType.Kill);
        var killScore = _immutableData.KillScoreData[killIndex]._getScore;
        var helpIndex = _immutableData.GetRankIndex(_immutableData.GetScoreValue(ScoreType.Help, _helpScoreID), ScoreType.Help);
        var helpScore = _immutableData.HelpScoreData[helpIndex]._getScore;
        _additionalPoint = killScore + helpScore;
        // 合計値をセットする
        _latestTotalPoint = _previousTotalPoint + _additionalPoint;
    }
    public void SaveTotalValue()
    {
        Debug.Log("値を保存します");
        // 合計値をPlayerPrefsに保存する
        PlayerPrefs.SetInt(_totalScoreKey, _latestTotalPoint);
    }
}
