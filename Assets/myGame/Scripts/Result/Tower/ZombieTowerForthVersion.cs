using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ZombieTowerForthVersion : MonoBehaviour
{
    [Header("実行に必要な値群")]
    [SerializeField]
    private ResultController _resultController = default;
    [SerializeField]
    private GameObject _zombiePrefab = default;
    [SerializeField]
    private RectTransform _zombieRectTransform = default;
    [SerializeField]
    private Transform _zombieParentCanvas = default;
    [SerializeField]
    private Transform _generatePos = default;
    [SerializeField]
    private Transform _cameraTarget = default;
    [SerializeField]
    private GameObject _stringGroup = default;
    [Tooltip("x軸スケール調整用")]
    [SerializeField]
    private float _scaleAdjustmentX = 1f;
    [Tooltip("y軸スケール調整用")]
    [SerializeField]
    private float _scaleAdjustmentY = 1f;
    [Tooltip("最大高さ")]
    [SerializeField]
    private float _maxHeight = default;
    [Tooltip("最底辺の長さ")]
    [SerializeField]
    private float _maxWidth = 1f;
    [Tooltip("次の要素の値まで,現在のインデックス+1の数につき1つ ピラミッドに積むインスタンスを生成する。")]
    [SerializeField]
    private int[] _deceptionBorderline = default;
    [SerializeField]
    private ResultImmutableData _immutableData = default;
    [Tooltip("村人たちの親オブジェクト"), SerializeField]
    private Transform _villagersParentObj = default;
    [Tooltip("プレイヤーのアニメーター"), SerializeField]
    private Animator _playerAnimator = default;

    [Header("調整用の値")]
    [SerializeField]
    private float _zombieFallAnimDurationTime = 1f;
    [SerializeField]
    private Ease _zombieFallAnimEase = default;
    [SerializeField]
    private float _cameraTopWaitTime = 1f;
    [SerializeField]
    private float _cameraFallAnimDurationTime = 3f;
    [SerializeField]
    private Ease _cameraFallAnimEase = default;
    [SerializeField]
    private float _generateInterval = 0.1f;
    [SerializeField]
    private float _randomPosDiffMin;
    [SerializeField]
    private float _randomPosDiffMax;
    [SerializeField]
    private float _cameraDurationTime = 1f;
    [SerializeField]
    private float _zombieHeightDiff = 50f;
    [SerializeField]
    private float _zombieWidthDiff = 50f;
    [SerializeField]
    private Vector2 _generateOffset = default;
    [Tooltip("早送りボタンが押されている時の加速度"), SerializeField, Range(1f, 10f)]
    private float _pyramidSpeedAcceleration = 1f;

    /// <summary> 生成するX座標のスタック </summary>
    private Stack<int> _generatePosXStack = new Stack<int>();
    /// <summary> この行にいくつ生成するか記憶しておくスタック </summary>
    private Stack<int> _generateNumberStack = new Stack<int>();

    private void Start()
    {
        // 表示する村人の数を調整
        Debug.Log($"助けた村人の数{_immutableData.GetScoreValue(ScoreType.Help, 12)}");
        for (int i = 0; i < _villagersParentObj.childCount; i++)
        {
            if (i < _immutableData.GetScoreValue(ScoreType.Help, 12))
            {
                _villagersParentObj.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                _villagersParentObj.GetChild(i).gameObject.SetActive(false);
            }
        }
        // 演出再生処理
        // 演出の流れ : ピラミッド演出→村人ダンス演出→評価演出再生
        StartCoroutine(PlaySecondVersion(
            () => StartCoroutine(VillagerAnimStart(
                () => { _stringGroup.SetActive(true); _resultController.IsEnd = true; }))));
    }
    private bool _previousIsFastForward = false;
    private bool _previousIsSkip = false;
    private void Update()
    {
        if (_previousIsFastForward != _resultController.IsFastForward)
        {
            ChangeDanceAnimSpeed(_resultController.CurrentPlaySpeed);
        }
        if (_resultController.IsSkip)
        {
            ChangeDanceAnimSpeed(999999f);
        }
        _previousIsFastForward = _resultController.IsFastForward;
    }

    public IEnumerator PlaySecondVersion(System.Action onComplete = default)
    {
        // 生成済みの数を表す値
        int generatedNumber = 0;
        // 生成するゾンビの数を取得
        int generateNumber = AdjustKillValue(_immutableData.GetScoreValue(ScoreType.Kill, 0));
        // 上昇演出後、開始位置まで降下するので、開始位置を記憶しておく
        float startPosY = _cameraTarget.position.y;
        // 生成する位置をスタックする。
        for (int currentLineNum = GetMaxLineNumber(); currentLineNum > -1 && generatedNumber < generateNumber; currentLineNum--)
        {
            int[] shufflePosX = ArrayShuffle(GetSetupArray(GetOneLineGenerateNumber(currentLineNum), currentLineNum / (int)Mathf.Round(Mathf.Sqrt(GetMaxLineNumber()))));
            _generateNumberStack.Push(shufflePosX.Length);
            for (int i = 0; i < shufflePosX.Length && generatedNumber < generateNumber; i++)
            {
                _generatePosXStack.Push(shufflePosX[i]);
                generatedNumber++;
            }
        }
        // カウンター変数を初期化
        generatedNumber = 0;
        // 生成した列の数をカウントする変数
        int generateLineCounter = 0;
        // スプライトを取得する
        var sprites = _immutableData.GetKillScoreShuffleSprites();
        // スタックした位置に向かってゾンビを生成する
        for (int currentLineNum = 0; currentLineNum < GetMaxLineNumber() && generatedNumber < generateNumber; currentLineNum++)
        {
            generateLineCounter++;
            // カメラの位置を更新する
            UpdateCameraPosUp(currentLineNum);
            // この行に生成する数を取得する
            var maxGenerateNumber = _generateNumberStack.Pop();
            // 各ゾンビを生成する
            for (int generateNumberOfThisLine = 0; generateNumberOfThisLine < maxGenerateNumber && generatedNumber < generateNumber; generateNumberOfThisLine++)
            {
                // ゾンビの生成座標の取得処理
                var generatePos = new Vector3
                        (
                            GetGeneratePosX(_generatePosXStack.Pop(), true),
                            GetGeneratePosY(),
                            0f
                        );
                // ゾンビの生成処理
                if (generatedNumber < sprites.Length) // 一時的な回避処理
                {
                    GenerateZombie(generatePos, currentLineNum, sprites[generatedNumber]);
                }
                else
                {
                    Debug.Log($"無効なIndexが指定されました。{generatedNumber}は範囲外です！");
                }
                // 生成済みのゾンビをカウントする
                generatedNumber++;
                if (!_resultController.IsSkip) // スキップしないときは待機する。
                {
                    // 待機
                    yield return new WaitForSeconds(_generateInterval / _resultController.CurrentPlaySpeed);
                }
            }
        }

        // 頂点にて指定時間、カメラの位置を待機させる。
        yield return new WaitForSeconds(_cameraTopWaitTime / _resultController.CurrentPlaySpeed);
        // 降下演出を再生
        UpdateCameraPosDown(startPosY, onComplete, (float)generateLineCounter / (float)GetMaxLineNumber());
    }
    #region Geter
    /// <summary> 指定の列の底辺の長さを取得する </summary>
    /// <param name="targetLine"> 対象の列（一番底が0としてカウントアップする想定で作製したメソッド） </param>
    private float GetBaseLength(int targetLine)
    {
        return (((float)GetMaxLineNumber() - (float)targetLine) / (float)GetMaxLineNumber()) * _maxWidth;
    }
    /// <summary> 1行に生成する数 </summary>
    /// <param name="targetLine"> 対象の列（一番底が0としてカウントアップする想定で作製したメソッド） </param>
    private int GetOneLineGenerateNumber(int targetLine)
    {
        //            底辺の長さ               / ゾンビ一体あたりの幅
        return (int)(GetBaseLength(targetLine) / GetZombieWidth());
    }
    /// <summary> 生成できる行の最大数 </summary>
    private int GetMaxLineNumber()
    {
        //            最大高さ  / ゾンビ一体あたりの高さ
        return (int)(_maxHeight / GetZombieHeight());
    }
    private float GetTargetPosYRandom(int targetLine)
    {
        return targetLine * GetZombieHeight() * _scaleAdjustmentY - UnityEngine.Random.Range(_randomPosDiffMin, _randomPosDiffMax) + _generateOffset.y;
    }
    private float GetTargetPosYSimple(int targetLine)
    {
        return targetLine * GetZombieHeight() * _scaleAdjustmentY + _generateOffset.y;
    }
    private float GetZombieHeight()
    {
        return _zombieRectTransform.rect.height - _zombieHeightDiff;
    }
    private float GetZombieWidth()
    {
        return _zombieRectTransform.rect.width - _zombieWidthDiff;
    }
    private float GetGeneratePosX(int index, bool isHalfShift)
    {
        var Pos = index * GetZombieWidth() * _scaleAdjustmentX - UnityEngine.Random.Range(_randomPosDiffMin, _randomPosDiffMax);
        if (isHalfShift)
            return Pos + GetZombieWidth() / 2f;
        else
            return Pos;
    }
    private float GetGeneratePosY()
    {
        return _generatePos.position.y;
    }
    /// <summary>
    /// 指定された値を調整し、ゾンビ積み上げ演出に使用する値を返すメソッド。
    /// </summary>
    private int AdjustKillValue(int targetValue)
    {
        int result = 0; // 結果用変数
        for (int i = 0; i < _deceptionBorderline.Length; i++)
        {
            if (targetValue > _deceptionBorderline[i])
            {
                if (i + 1 < _deceptionBorderline.Length)
                {
                    result += (_deceptionBorderline[i + 1] - _deceptionBorderline[i]) / (i + 1);
                }
                else
                {
                    result += (targetValue - _deceptionBorderline[i]) / (i + 1);
                    break;
                }
            }
            else
            {
                if (i == 0)
                {
                    break;
                }
                else
                {
                    result += (targetValue - _deceptionBorderline[i]) / i;
                    break;
                }
            }
        }
        return result;
    }
    #endregion
    private TweenerCore<Vector3, Vector3, VectorOptions> _cameraAnim;
    /// <summary> カメラの上昇処理 </summary>
    private void UpdateCameraPosUp(int targetLine)
    {
        if (GetTargetPosYSimple(targetLine) > _cameraTarget.position.y)
        {
            if (_cameraAnim != null)
            {
                _cameraAnim.Kill();
            }
            _cameraAnim = _cameraTarget.transform.DOMoveY(GetTargetPosYSimple(targetLine), _cameraDurationTime / _resultController.CurrentPlaySpeed).
                SetEase(Ease.Linear).
                OnComplete(() => _cameraAnim = null);
        }
    }
    /// <summary> カメラの降下処理 </summary>
    private void UpdateCameraPosDown(float standardPosY, System.Action onComplete, float timeRate)
    {
        _cameraTarget.
            DOMoveY(standardPosY, _cameraFallAnimDurationTime * timeRate).
            SetEase(_cameraFallAnimEase).
            OnComplete(() => onComplete?.Invoke());
    }
    // 配列の要素をシャッフルする方法 https://dobon.net/vb/dotnet/programing/arrayshuffle.html
    // バブルソートのように並び変えるが、条件を指定せずランダムに並び変える。
    /// <summary> 配列の要素をシャッフルし戻り値として返す。 </summary>
    private int[] ArrayShuffle(int[] target)
    {
        int[] result = new int[target.Length];
        Array.Copy(target, result, target.Length);

        System.Random rng = new System.Random();
        int n = result.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int tmp = result[k];
            result[k] = result[n];
            result[n] = tmp;
        }
        return result;
    }
    /// <summary> 要素をセットアップ済みの配列を取得する </summary>
    private int[] GetSetupArray(int length, int startNum)
    {
        int[] result = new int[length];
        for (int i = 0, value = startNum; i < length; i++, startNum++)
        {
            result[i] = startNum;
        }
        return result;
    }
    /// <summary> ゾンビの生成処理 </summary>
    private void GenerateZombie(Vector3 generatePos, int targetLine)
    {
        if (!_resultController.IsSkip) // スキップしないときは待機する。
        {
            // ゾンビの生成処理
            var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);

            // 座標の調整
            var localPos = zombie.transform.localPosition;
            localPos.z = 0f;
            localPos.x += _generateOffset.x;
            zombie.transform.localPosition = localPos;
            // ランダムなz軸回転の設定
            zombie.GetComponent<RectTransform>().
                Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
            // 落下アニメーションの再生
            zombie.transform.
                DOMoveY(GetTargetPosYRandom(targetLine), _zombieFallAnimDurationTime).
                SetEase(_zombieFallAnimEase);
        }
        else
        {
            Instantiate(_zombiePrefab, new Vector3(generatePos.x,
                GetTargetPosYRandom(targetLine), generatePos.z),
                Quaternion.identity, _zombieParentCanvas);
        }
    }
    /// <summary> ゾンビの生成処理 </summary>
    private void GenerateZombie(Vector3 generatePos, int targetLine, Sprite sprite)
    {
        if (!_resultController.IsSkip) // スキップしないときは待機する。
        {
            // ゾンビの生成処理
            var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);

            // スプライトの割り当て
            zombie.GetComponent<Image>().sprite = sprite;
            // 座標の調整
            var localPos = zombie.transform.localPosition;
            localPos.z = 0f;
            localPos.x += _generateOffset.x;
            zombie.transform.localPosition = localPos;
            // ランダムなz軸回転の設定
            zombie.GetComponent<RectTransform>().
                Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
            // 落下アニメーションの再生
            zombie.transform.
                DOMoveY(GetTargetPosYRandom(targetLine), _zombieFallAnimDurationTime / _resultController.CurrentPlaySpeed).
                SetEase(_zombieFallAnimEase);
        }
        else
        {
            var zombie = Instantiate(_zombiePrefab, new Vector3(generatePos.x, GetTargetPosYRandom(targetLine),
                generatePos.z), Quaternion.identity, _zombieParentCanvas);

            // スプライトの割り当て
            zombie.GetComponent<Image>().sprite = sprite;
            // 座標の調整
            var localPos = zombie.transform.localPosition;
            localPos.z = 0f;
            localPos.x += _generateOffset.x;
            zombie.transform.localPosition = localPos;
            // ランダムなz軸回転の設定
            zombie.GetComponent<RectTransform>().
                Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
        }
    }
    /// <summary>
    /// 村人ダンスを再生する
    /// </summary>
    /// <param name="onComplete"> ダンス完了時に実行するアクション </param>
    private IEnumerator VillagerAnimStart(System.Action onComplete)
    {
        if (!_resultController.IsSkip)
        {
            // プレイヤーのダンスを開始する
            _playerAnimator.SetBool("DanceStart", true);
            // アニメーション終了検出用コンポーネントを取得する。
            var endDetection = _playerAnimator.GetComponent<VillagerAnimEndDetection>();
            for (int i = 0; i < _villagersParentObj.childCount; i++)
            {
                // 助けた数だけループする
                if (i < _immutableData.GetScoreValue(ScoreType.Help, 12))
                {
                    // 村人のダンスアニメーションを開始する
                    _villagersParentObj.GetChild(i).GetComponent<Animator>().SetBool("DanceStart", true);
                    if (endDetection == null) // プレイヤーがアニメーション完了用コンポーネントを所持していない可能性を想定。
                                              // 所持していない場合、村人から取得する。
                    {
                        endDetection = _villagersParentObj.GetChild(i).GetComponent<VillagerAnimEndDetection>();
                    }
                }
                else
                {
                    break;
                }
            }
            // アニメーションが完了するまで待機する
            // 条件が false の時, 待機する。
            yield return new WaitUntil(() => endDetection.IsAnimEnd);
        }

        onComplete?.Invoke();
    }
    /// <summary>
    /// ダンスアニメーションスピードを変更する
    /// </summary>
    private void ChangeDanceAnimSpeed(float animSpeed)
    {
        _playerAnimator.SetFloat("DanceSpeed", animSpeed);

        for (int i = 0; i < _villagersParentObj.childCount; i++)
        {
            // 助けた数だけループする
            if (i < _immutableData.GetScoreValue(ScoreType.Help, 12))
            {
                _villagersParentObj.GetChild(i).GetComponent<Animator>().SetFloat("DanceSpeed", animSpeed);
            }
            else
            {
                break;
            }
        }
    }
}