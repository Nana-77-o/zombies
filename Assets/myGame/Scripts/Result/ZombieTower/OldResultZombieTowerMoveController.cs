using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 背景のスクロール。パーティクルの制御をするクラス。
/// ※※※※※ 現在は使用していません ※※※※※
/// </summary>
public class OldResultZombieTowerMoveController : MonoBehaviour
{
    [Tooltip("背景の上昇スクロールに掛ける時間")]
    [SerializeField]
    private float _startDelayTime = 1f;
    [Tooltip("背景の上昇スクロールに掛ける時間")]
    [SerializeField]
    private float _scrollUpAnimationTime = 8f;
    [Tooltip("アニメーションのイージング")]
    [SerializeField]
    private Ease _scrollUpEase = default;
    [Tooltip("ゾンビのパーティクルを停止させるまでの時間")]
    [SerializeField]
    private float _particleStopTime = 8f;
    [Tooltip("ゾンビのパーティクルオブジェクト")]
    [SerializeField]
    private ParticleSystem _particle = null;

    [SerializeField]
    [Tooltip("上昇完了後、停止する時間")]
    private float _delayTime = default;
    [SerializeField]
    [Tooltip("降下スクロールに掛ける時間")]
    private float _scrollDownTime = 3f;
    [SerializeField]
    [Tooltip("降下スクロールのイージング")]
    private Ease _scrollDownEase = default;

    [Tooltip("降下完了後に表示する評価オブジェクト")]
    [SerializeField]
    private GameObject _evaluationDrawer = default;

    /// <summary> テスト用倒した数 </summary>
    public int _testKillValue = 15000;

    /// <summary> 倒したゾンビの数 </summary>
    private int _killValue = 0;

    public float AnimationTime => _scrollUpAnimationTime;
    public Ease ScrollUpEase => _scrollUpEase;

    //private void Start()
    //{
    //    _killValue = _testKillValue; // テスト用処理

    //    StartAnimation();
    //}

    //private async void StartAnimation()
    //{
    //    await Task.Delay((int)(_startDelayTime * 1000));
    //    // パーティクルの停止用コルーチンを開始
    //    StartCoroutine(ParticleTimer());
    //    // 称号を取得する。
    //    var title = _killValue / 100f;
    //    // 20より大きければ20にする。これで0～20（称号の段階）を表す整数値を取得できた。
    //    title = title > 20f ? 20f : title;
    //    // 目的の位置になるように値を加工する。
    //    var yPos = title * -486f;
    //    // その位置まで移動する。
    //    transform.
    //        DOLocalMoveY(yPos, _scrollUpAnimationTime).
    //        SetEase(_scrollUpEase).
    //        OnComplete(() => ReturnToOriginalPosition());
    //}
    ///// <summary>
    ///// 基の位置に戻る
    ///// </summary>
    //private void ReturnToOriginalPosition()
    //{
    //    transform.
    //        DOLocalMoveY(0f, _scrollDownTime).// 元いた位置までスクロールする
    //        SetEase(_scrollDownEase).         // イージングを設定する
    //        SetDelay(_delayTime).             // 遅延処理
    //        OnComplete(() => _evaluationDrawer.SetActive(true)); // 評価を表示する
    //}

    //private IEnumerator ParticleTimer()
    //{
    //    yield return new WaitForSeconds(_particleStopTime);
    //    _particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //}
}