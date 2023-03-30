using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZombieTowerSecondVersion : MonoBehaviour
{
    [SerializeField]
    private float _testGenerateInterval = 0.3f;
    [SerializeField]
    private float _testZombieFallAnimationDuration = 1f;
    [SerializeField]
    private int _testKillScore;

    [SerializeField]
    private RectTransform _towerRectTransform = default;
    [SerializeField]
    private RectTransform _zombieRect = default;
    [SerializeField]
    private ResultImmutableData _immutableData = default;
    [SerializeField]
    private Transform _GenerateCanvas = default;
    [SerializeField]
    private GameObject _zombiePrefab = default;
    [SerializeField]
    private Transform _generatePos = default;

    private void Update()
    {
        // Test
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("テスト開始");
            StartCoroutine(AnimationPlay(_testKillScore, _testGenerateInterval));
        }
    }
    [SerializeField]
    private float _randomPosDiffMax = 1f;
    [SerializeField]
    private float _randomPosDiffMin = -1f;
    [SerializeField]
    private float _reduceDiffY = 1f;
    private IEnumerator AnimationPlay(int killScoreValue, float generateInterval)
    {
        var oneLineGenerateValue =
            (int)_towerRectTransform.rect.width / (int)_zombieRect.sizeDelta.x; // 一列に生成するゾンビの数

        float oneLineHeight = _zombieRect.sizeDelta.y - _reduceDiffY; // 一行辺りの高さ
        // 倒した数だけループする。
        var generateCounter = 0;

        // 生成位置(x成分)用の値
        float generatePosX;
        while (generateCounter < killScoreValue)
        {
            for (LoopInitialization(oneLineHeight); _generateCounter < oneLineGenerateValue + 1 && generateCounter < killScoreValue; _generateCounter++, generateCounter++)// 偶数列に必要な数生成する
            {
                generatePosX =
                    _generateCounter * _zombieRect.rect.width +
                    _towerRectTransform.position.x - _towerRectTransform.sizeDelta.x / 2f; // 生成位置(x成分)を取得
                Debug.Log($"生成位置xは{generatePosX}です");
                // 生成する。
                var zombie = ZombieGeneration(generatePosX);
                var rt = zombie.GetComponent<RectTransform>();
                rt.localPosition =
                    new Vector3(
                        rt.localPosition.x - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        rt.localPosition.y - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        0f);
                // z軸回転をランダムに設定する
                var angleZ = Random.Range(0, 360);
                rt.Rotate(0f, 0f, angleZ);
                // 生成物のアニメーションを実行する。
                ZombieAnimation(zombie.transform, _targetPosY);
                // 指定時間待つ。
                yield return new WaitForSeconds(generateInterval);
            }
            for (LoopInitialization(oneLineHeight);
                _generateCounter < oneLineGenerateValue && generateCounter < killScoreValue;
                _generateCounter++, generateCounter++) // 奇数列に必要な数生成する
            {
                generatePosX =
                    _generateCounter * _zombieRect.rect.width +
                    _zombieRect.sizeDelta.x / 2f +
                    _towerRectTransform.position.x - _towerRectTransform.sizeDelta.x / 2f; // 生成位置(x成分)を取得
                Debug.Log($"生成位置xは{generatePosX}です");
                // 生成する。
                var zombie = ZombieGeneration(generatePosX);
                // 位置、回転の調整を行う
                // x,y座標に少しだけ遊びを持たせる
                var rt = zombie.GetComponent<RectTransform>();
                rt.localPosition =
                    new Vector3(
                        rt.localPosition.x - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        rt.localPosition.y - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        0f);
                // z軸回転をランダムに設定する
                var angleZ = Random.Range(0, 360);
                rt.Rotate(0f, 0f, angleZ);
                // 生成物のアニメーションを実行する。
                ZombieAnimation(zombie.transform, _targetPosY);
                // 指定時間待つ。
                yield return new WaitForSeconds(generateInterval);
            }
        }
    }

    private int _generateCounter = 0;
    private float _targetPosY = 0f;

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3,
        DG.Tweening.Plugins.Options.VectorOptions> _cameraAnim = default;

    [SerializeField]
    private Ease _cameraAnimEase = Ease.Linear;
    [SerializeField]
    private float _cameraAnimDuration = 1f;

    private void LoopInitialization(float oneLineHeight)
    {
        _generateCounter = 0; // for文で使用する値を初期化
        _targetPosY += oneLineHeight; // 目標地点(y座標)を更新
        if (_targetPosY > Camera.main.transform.position.y) // 目標地点の高さがカメラより高ければカメラの位置を更新する
        {
            // アニメーション再生中であればキルする。
            _cameraAnim?.Kill();
            // アニメーションを再生する。
            _cameraAnim = Camera.main.transform.
                DOMoveY(_targetPosY * _GenerateCanvas.localScale.y, _cameraAnimDuration).
                SetEase(_cameraAnimEase).
                OnComplete(() => _cameraAnim = null);
        }
    }
    private GameObject ZombieGeneration(float generatePosX)
    {
        return Instantiate(_zombiePrefab,
            new Vector3(generatePosX * _GenerateCanvas.localScale.x
            , _generatePos.position.y
            , 0f),
            Quaternion.identity,
            _GenerateCanvas);
    }
    [SerializeField]
    private Ease _zombieFallAnimEase = default;
    private void ZombieAnimation(Transform zombie, float targetPosY)
    {
        zombie.DOMoveY(targetPosY * _GenerateCanvas.localScale.y,
            _testZombieFallAnimationDuration).
            SetEase(_zombieFallAnimEase);
    }

}
