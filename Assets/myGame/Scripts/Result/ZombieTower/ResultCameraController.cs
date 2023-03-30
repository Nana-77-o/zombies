using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[System.Serializable]
public class ResultCameraController : MonoBehaviour
{
    //[Tooltip("カメラの下限位置"), SerializeField]
    //private float _cameraMinPosY = -4860;
    //[Tooltip("カメラの上限位置"), SerializeField]
    //private float _cameraMaxPosY = 4860;

    //private float _zombieTowerTop = -5400f; // 初期値は手計算で出しました。
    //private Transform _cameraTransform = default;
    //private float _targetPosY = default;

    //private ResultZombieTowerFillController _fillController = null;

    //public void Init(ResultZombieTowerFillController fillController)
    //{
    //    _cameraTransform = Camera.main.transform;
    //    _fillController = fillController;
    //    _startPos = transform.localPosition;
    //    StartCameraPosGoUp();
    //}
    ///// <summary>
    ///// カメラの位置の上昇処理
    ///// </summary>
    //private async void StartCameraPosGoUp()
    //{
    //    _targetPosY = (_fillController.TargetTitle - 10) * 540f;
    //    await Task.Delay((int)(_fillController.StartDelayTime * 1000));
    //    // ゾンビタワーと同じイージング・時間で上昇する（_zombieTowerTopを同じように更新する。）
    //    transform.DOLocalMoveY(_targetPosY, _fillController.AnimationTime).
    //        SetEase(_fillController.AnimationEasing).
    //        OnComplete(() => StartCameraPosGoDown());
    //}
    //private Vector2 _startPos = default;
    //[Tooltip("降下アニメーションの遅延時間"), SerializeField]
    //private float _downDelayTime = 0f;
    //[Tooltip("降下に掛ける時間"), SerializeField]
    //private float _downAnimationTime = 1f;
    //[Tooltip("降下アニメーションのイージング"), SerializeField]
    //private Ease _downAnimationEasing = default;
    //[SerializeField]
    //private GameObject _stringGroup = default;
    ///// <summary>
    ///// カメラの降下処理
    ///// </summary>
    //private async void StartCameraPosGoDown()
    //{
    //    await Task.Delay((int)(_downDelayTime * 1000));
    //    transform.DOLocalMoveY(_startPos.y, _downAnimationTime).
    //        SetEase(_downAnimationEasing).
    //        OnComplete(() => _stringGroup.SetActive(true));
    //}
}
