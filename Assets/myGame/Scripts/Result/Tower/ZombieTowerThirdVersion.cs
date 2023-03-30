using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class ZombieTowerThirdVersion : MonoBehaviour
{
    [Header("実行に必要な値群")]
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
    [Header("テスト用にインスペクタウィンドウに表示したい値")]
    private float _testValue;
    
    private void Start()
    {
        StartCoroutine(Play(() => _stringGroup.SetActive(true)));
    }

    /// <summary> 演出の再生処理 </summary>
    /// <param name = "onComplete" > 完了時に実行したい処理 </ param >
    public IEnumerator Play(System.Action onComplete = default)
    {
        int generatedNumber = 0; // 生成した数を表す値
        int killNumber = _immutableData.GetScoreValue(ScoreType.Kill, 0);
        float startPosY = _cameraTarget.position.y;
        // 最大列数だけループする // 生成数の数ループする
        for (int currentLineNum = 0; currentLineNum < GetMaxLineNumber() && generatedNumber < killNumber; currentLineNum++)
        {
            UpdateCameraPosUp(currentLineNum);
            // "0"から"一列に生成する最大数"までの整数値が シャッフルされた配列を取得する。
            // 例 : 一列に3体生成する場合 {0, 2, 1}, 一列に5体生成する場合 {4, 3, 0, 1, 2}
            // Mathf.Sqrt(GetMaxLineNumber())なぜこのような動作になるのかよくわかっていない。たまたまうまく動作した。調査の必要有り。
            int[] shufflePosX = ArrayShuffle(GetSetupArray(GetOneLineGenerateNumber(currentLineNum), currentLineNum / (int)Mathf.Round(Mathf.Sqrt(GetMaxLineNumber()))));
            // 各列にゾンビを生成する // 生成数の数ループする
            for (int generateNumber = 0; generateNumber < shufflePosX.Length && generatedNumber < killNumber; generateNumber++)
            {
                // ゾンビの生成座標の取得処理
                var generatePos = new Vector3
                        (
                            GetGeneratePosX(shufflePosX[generateNumber], true),
                            GetGeneratePosY(),
                            0f
                        );
                // ゾンビの生成処理
                var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);
                generatedNumber++; // 生成した数をカウントする。
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
                    DOMoveY(GetTargetPosYRandom(currentLineNum), _zombieFallAnimDurationTime).
                    SetEase(_zombieFallAnimEase);
                // 待機
                yield return new WaitForSeconds(_generateInterval);
            }
        }
        yield return new WaitForSeconds(_cameraTopWaitTime);
        // 降下演出を再生
        UpdateCameraPosDown(startPosY, onComplete);
    }
    #region Geter
    /// <summary> 指定の列の底辺の長さを取得する </summary>
    /// <param name="targetLine"> 対象の列（一番底が0としてカウントアップする想定で作製したメソッド） </param>
    private float GetBaseLength(int targetLine)
    {
        return (((float)GetMaxLineNumber() - (float)targetLine) / (float)GetMaxLineNumber()) * _maxWidth;
    }
    /// <summary> 1列に生成する数 </summary>
    /// <param name="targetLine"> 対象の列（一番底が0としてカウントアップする想定で作製したメソッド） </param>
    private int GetOneLineGenerateNumber(int targetLine)
    {
        return (int)(GetBaseLength(targetLine) / GetZombieWidth());
    }
    /// <summary> 列の最大数 </summary>
    private int GetMaxLineNumber()
    {
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
            _cameraAnim = _cameraTarget.transform.DOMoveY(GetTargetPosYRandom(targetLine), _cameraDurationTime).
                SetEase(Ease.Linear).
                OnComplete(() => _cameraAnim = null);
        }
    }
    /// <summary> カメラの降下処理 </summary>
    private void UpdateCameraPosDown(float standardPosY, System.Action onComplete)
    {
        _cameraTarget.
            DOMoveY(standardPosY, _cameraFallAnimDurationTime).
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
}