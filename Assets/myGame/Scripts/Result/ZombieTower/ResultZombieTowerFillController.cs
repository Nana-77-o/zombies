using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゾンビタワーの表示エリア(Fill amount)を制御するクラス
/// </summary>
public class ResultZombieTowerFillController : MonoBehaviour
{
    //[Tooltip("ゾンビタワー上昇開始までの遅延時間")]
    //[SerializeField]
    //private float _startDelayTime = 1f;
    //[Tooltip("ゾンビタワーの上昇アニメーションのイージング")]
    //[SerializeField]
    //private Ease _animationEasing = default;
    //[Tooltip("ゾンビのパーティクル")]
    //[SerializeField]
    //private ParticleSystem _zombieParticle = default;
    //[SerializeField]
    //private ShowRatingsInOrder _data = default;
    //[SerializeField]
    //private ResultImmutableData _immutableData = default;

    //private int _targetTitle = default;

    ///// <summary>
    ///// ゾンビを積み上げる演出に掛ける時間を表現する値
    ///// </summary>
    //private float _animationTime = default;
    //public float StartDelayTime => _startDelayTime;
    //public float AnimationTime => _animationTime;
    //public Ease AnimationEasing => _animationEasing;
    //public int TargetTitle => _targetTitle;

    ///// <summary>
    ///// 倒したゾンビの数
    ///// </summary>
    //private int _killValue = 0;

    //private void Start()
    //{
    //    _killValue = TestVariable._testKillNumber; // テスト用処理
    //    StartAnimation();
    //    transform.GetChild(0).GetComponent<ResultCameraController>().Init(this);
    //}
    //private async void StartAnimation()
    //{
    //    // 称号を取得する。
    //    _targetTitle = _killValue / 100;
    //    // 20より大きければ20にする。これで0～20（称号の段階）を表す整数値を取得できた。
    //    _targetTitle = _targetTitle > 20 ? 20 : _targetTitle;
    //    await Task.Delay((int)(_startDelayTime * 1000));
    //    int killIndex = _immutableData.GetRankIndex(TestVariable._testKillNumber, ScoreType.Kill);
    //    //_animationTime = _data.KillScore[GetAnimIndex(_killValue)]._pileUpTime; // アニメーションに掛ける時間を取得する
    //    _animationTime = _immutableData.KillScoreData[killIndex]._pileUpTime;
    //    // フィルを変更する。
    //    var image = GetComponent<Image>();
    //    DOTween.To(
    //    () => image.fillAmount,
    //    (x) => image.fillAmount = x,
    //    _targetTitle / 20f,
    //    _animationTime).
    //    SetEase(_animationEasing).
    //    OnComplete(() => StopZombieParticle());
    //}
    //[SerializeField]
    //float _particleStopDelayTime = 1f;
    //private async void StopZombieParticle()
    //{
    //    await Task.Delay((int)(_particleStopDelayTime * 1000));
    //    _zombieParticle.Stop();
    //}
}
