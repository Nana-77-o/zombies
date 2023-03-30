using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// 順番に評価を表示するクラス
/// </summary>
public class ShowRatingsInOrder : MonoBehaviour
{
    [Header("スコアテキスト : テスト用")]
    [SerializeField]
    private Text _testKillString = default;
    [SerializeField]
    private Text _testHelpString = default;
    [SerializeField]
    private Text _testTotalString = default;
    [SerializeField]
    private Text _testKillNumber = default;
    [SerializeField]
    private Text _testHelpNumber = default;
    [Header("スコアイメージ")]
    [SerializeField]
    private Image _killScoreImage = default;
    [SerializeField]
    private Image _helpScoreImage = default;
    [SerializeField]
    private Image _totalScoreImage = default;
    [Header("データ")]
    [SerializeField]
    private ResultImmutableData _immutableData = default;
    [SerializeField]
    private ResultVariableData _variableData = default;
    [Header("オーディオ")]
    [SerializeField]
    private ResultSEController _soundEffectController = default;
    [Header("関連する他のオブジェクト群")]
    [SerializeField]
    private GaugeAnimationSample _gaugeController = default;
    [SerializeField]
    private GameObject _buttons = default;
    [SerializeField]
    private ResultDisplay[] _displays = default;

    /// <summary>
    /// 表示するイメージのインデックス
    /// </summary>
    private int _displayIndex = 0;

    private void Awake()
    {
        foreach (var e in _displays)
        {
            e._display.SetActive(false);
        }
        _buttons.SetActive(false);
        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        _variableData.UpdateValue();
        SetScoreText();
        DisplayInOrder();
    }
    /// <summary>
    /// 順番に表示する。
    /// </summary>
    public async void DisplayInOrder()
    {
        // 指定番目を表示する。
        _displays[_displayIndex]._display.SetActive(true);
        // SEを鳴らす
        //_soundEffectController.PlaySE(0);
        // delayTime分待つ
        await Task.Delay(_displays[_displayIndex]._delayTime);
        // 指定インデックスをインクリメントして
        // 範囲内であれば再起処理する。
        // 範囲外であればボタンを表示し終了する。
        _displayIndex++;
        if (_displayIndex < _displays.Length)
        {
            DisplayInOrder();
        }
        else
        {
            // SEを鳴らし、ボタンを表示する。
            //_soundEffectController.PlaySE(1);
            //_buttons.SetActive(true);
            _gaugeController.gameObject.SetActive(true);
            return;
        }
    }
    public async void SetScoreText()
    {
        // 討伐数の称号を取得する
        int killIndex = _immutableData.GetScoreIndex(ScoreType.Kill);
        _killScoreImage.sprite = _immutableData.KillScoreData[killIndex]._degreeImage;
        // 救出数の称号を取得する
        int helpIndex = _immutableData.GetScoreIndex(ScoreType.Help);
        _helpScoreImage.sprite = _immutableData.HelpScoreData[helpIndex]._degreeImage;
        // トータルスコアの称号を取得する
        int totalIndex = _immutableData.GetRankIndex(_variableData.PreviousTotalPoint, ScoreType.Total);
        _totalScoreImage.sprite = _immutableData.TotalScoreData[totalIndex]._degreeImage;
        // 討伐数を設定する
        _testKillNumber.text = $"{_immutableData.GetScoreValue(ScoreType.Kill, 0).ToString("#,0")}";
        // 救出数を設定する
        _testHelpNumber.text = $"{_immutableData.GetScoreValue(ScoreType.Help, 12)}/10";

        await JsonProcessClass.TestSaveAchievements(killIndex, helpIndex, totalIndex);
    }
}
/// <summary>
/// 称号を表す構造体
/// </summary>
[System.Serializable]
public class ResultDisplay
{
    [Tooltip("表示するオブジェクト（上から順に表示する）")]
    [SerializeField]
    public GameObject _display = default;
    [Tooltip("次の評価を表示するまでの時間（ﾐﾘ秒）\n" +
        "最後の要素のディレイはボタン表示までの待ち時間")]
    [SerializeField]
    public int _delayTime = 1000;
}
[System.Serializable]
public struct Degree
{
    [Tooltip("必要スコア（討伐数,救出数,合計スコア）"), SerializeField]
    public int _needScore;
    [Tooltip("テスト用 : 称号名"), SerializeField]
    public string _testDegreeStrings;
    [Tooltip("称号画像"), SerializeField]
    public Sprite _degreeImage;
    [Tooltip("獲得スコア"), SerializeField]
    public int _getScore;
    [Tooltip("ゾンビを積み上げる演出に掛ける時間"), SerializeField]
    public float _pileUpTime;
}