using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 階級ゲージのアニメーションを制御するクラス
/// </summary>
public class GaugeAnimationSample : MonoBehaviour
{
    [Header("データ")]
    [SerializeField]
    private ResultImmutableData _immutableData = default;
    [SerializeField]
    private ResultVariableData _variableData = default;
    [Header("アニメーションに関する値")]
    [SerializeField]
    private float _animationTime = 3f;
    [Header("テキスト群")]
    [SerializeField]
    private Text _startText = default;
    [SerializeField]
    private Text _endText = default;
    [SerializeField]
    private Text _startPointText = default;
    [SerializeField]
    private Text _endPointText = default;
    [SerializeField]
    private Image _gaugeImage = default;
    [SerializeField]
    private Ease _animEase = default;
    [Header("ゲージ用イメージ")]
    [SerializeField]
    private Image _startImage = default;
    [SerializeField]
    private Image _endImage = default;
    [Tooltip("消えるまでの遅延時間"), SerializeField]
    private float _disableDelayTime = 1f;
    [SerializeField]
    private ShowRatingsInOrder _rankTextDrawer = default;
    [SerializeField]
    private GameObject _buttons = default;

    [Header("サウンド関連")]
    [SerializeField]
    private ResultSEController _seController = default;
    private bool _isFirstScroll = true;
    [SerializeField]
    private AudioClip _goodME = default;
    [SerializeField]
    private AudioClip _greatME = default;

    [Header("ポップアップ演出関連")]
    [SerializeField]
    private GameObject _popUpObjBG = default;
    [SerializeField]
    private GameObject _popUpObj = default;
    [SerializeField]
    private float _popUpAnimTime = 2.2f;
    [SerializeField]
    private float _popUpWaitTime = 0.8f;
    [SerializeField]
    private Ease _popUpAnimEase = Ease.OutBounce;

    [SerializeField]
    private Image _totalScoreImage = default;

    private void Awake()
    {
        // ポップアップオブジェクトのセットアップ処理
        SetUpPopUpObject();

        // 自身を非アクティブにする
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _variableData.UpdateValue();
        StartScroll();
    }

    public void StartScroll()
    {
        gameObject.SetActive(true);
        int index = _immutableData.GetRankIndex(_variableData.PreviousTotalPoint, ScoreType.Total);
        ScrollBar(index);
    }
    private void ScrollBar(int standardIndex)
    {
        // 範囲ないかどうかチェックする
        if (standardIndex > -1 && standardIndex + 1 < _immutableData.TotalScoreData.Length)
        {
            // 各テキストを設定する
            SetImage(_immutableData.TotalScoreData[standardIndex], _immutableData.TotalScoreData[standardIndex + 1]);

            // currentポイントからnextポイントまでに必要なポイントを取得する
            var diff = _immutableData.TotalScoreData[standardIndex + 1]._needScore - _immutableData.TotalScoreData[standardIndex]._needScore;
            // スタートポイントのFill Amount値を取得する
            // 必要なポイントを基に加算前の値の割合を求める
            var startFillAmount = (float)(_variableData.PreviousTotalPoint - _immutableData.TotalScoreData[standardIndex]._needScore) / (float)diff;
            // エンドポイントのFill Amount値を取得する
            // 必要なポイントを基に加算後の値の割合を求める
            var endFillAmount = (float)(_variableData.LatestTotalPoint - _immutableData.TotalScoreData[standardIndex]._needScore) / (float)diff;
            if (endFillAmount > 0.9999f)// 超えている場合
            {
                if (_isFirstScroll)
                {
                    _isFirstScroll = false;
                    // GreatMEを再生する。
                    _seController.PlaySE(0);
                }
                AnimationPlay(_animationTime / 2f, startFillAmount, 1f,
                    () => StartCoroutine(PopUp(standardIndex + 1,
                    () => ScrollBar(standardIndex + 1))));
            }
            else
            {
                if (_isFirstScroll)
                {
                    _isFirstScroll = false;
                    // GoodMEを再生する。
                    _seController.PlaySE(1);
                }
                System.Action onComplete = default;
                onComplete += () => StartCoroutine(DisableDelay(_disableDelayTime));
                AnimationPlay(_animationTime / 2f, startFillAmount, endFillAmount, onComplete);
                _variableData.SaveTotalValue();
                _variableData.UpdateValue();
            }
        }
        else
        {
            Debug.LogWarning("渡された値は範囲外です。");
            StartCoroutine(DisableDelay(0f)); // 待たないがDisable処理を実行しない為0を渡す。
            _variableData.SaveTotalValue();
            _variableData.UpdateValue();
        }
    }
    private void PlayAudio(AudioClip clip)
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Fillをアニメーションさせる
    /// </summary>
    /// <param name="animTime"> 再生時間 </param>
    /// <param name="startFill"> 開始時のFill値 </param>
    /// <param name="endFill"> 終了時のFill値 </param>
    private void AnimationPlay(float animTime, float startFill, float endFill, System.Action onComplete = null)
    {
        _gaugeImage.fillAmount = startFill;
        DOTween.To(
        () => _gaugeImage.fillAmount,
        (x) => _gaugeImage.fillAmount = x,
        endFill,
        animTime).
        SetEase(_animEase).
        OnComplete(() => onComplete?.Invoke());
    }
    /// <summary>
    /// 各テキストに値をセットする
    /// </summary>
    /// <param name="currentDegree"></param>
    /// <param name="nextDegree"></param>
    private void SetImage(Degree currentDegree, Degree? nextDegree = null)
    {
        // _startText.text = currentDegree._testDegreeStrings;
        _startImage.sprite = currentDegree._degreeImage;
        _startPointText.text = currentDegree._needScore.ToString();
        if (nextDegree != null)
        {
            // _endText.text = nextDegree?._testDegreeStrings;
            _endImage.sprite = nextDegree?._degreeImage;
            _endPointText.text = nextDegree?
                ._needScore.ToString();
        }
        else
        {
            _endText.text = "";
            _endPointText.text = "";
        }
    }
    /// <summary>
    /// 自身を非アクティブ化する際の処理と <br/>
    /// 自身を非アクティブ化する。
    /// </summary>
    /// <param name="delayTime"> 非アクティブ化するまでの遅延時間 </param>
    private IEnumerator DisableDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // _rankTextDrawer.SetScoreText(); // 不要な処理だと思われる。時間が経って問題ないと判断した場合削除する。(2023/02/26 記載)
        _buttons.SetActive(true);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ポップアップオブジェクトのセットアップ処理
    /// </summary>
    private void SetUpPopUpObject()
    {
        // スケールを小さくする
        _popUpObj.GetComponent<RectTransform>().localScale = Vector3.zero;
        _popUpObjBG.GetComponent<RectTransform>().localScale = Vector3.zero;
        // ポップアップオブジェクトを非アクティブにする
        _popUpObj.SetActive(false);
        _popUpObjBG.SetActive(false);
    }
    private IEnumerator PopUp(int nextIndex, System.Action onComplete)
    {
        Debug.Log("ポップアップ開始");
        // ポップアップ演出を再生する
        _popUpObj.SetActive(true);
        _popUpObjBG.SetActive(true);
        // イメージの設定
        _popUpObj.GetComponent<Image>().sprite = _immutableData.TotalScoreData[nextIndex]._degreeImage;
        // アニメーション再生
        var isAnimEnd = false;
        _popUpObj.GetComponent<RectTransform>().DOScale(Vector3.one, _popUpAnimTime).SetEase(_popUpAnimEase).OnComplete(() => isAnimEnd = true);
        _popUpObjBG.GetComponent<RectTransform>().DOScale(Vector3.one, _popUpAnimTime).SetEase(_popUpAnimEase);
        yield return new WaitWhile(() => !isAnimEnd); // アニメーションが完了するまで待機  
        yield return new WaitForSeconds(_popUpWaitTime);       // ボタン入力があるまで待機

        // イメージの更新
        _totalScoreImage.sprite = _immutableData.TotalScoreData[nextIndex]._degreeImage;
        // スケールの初期化
        _popUpObj.GetComponent<RectTransform>().localScale = Vector3.zero;
        _popUpObjBG.GetComponent<RectTransform>().localScale = Vector3.zero;
        // ポップアップ演出アニメーションオブジェクトを非表示にする
        _popUpObj.SetActive(false);
        _popUpObjBG.SetActive(false);
        onComplete?.Invoke();
    }
}
