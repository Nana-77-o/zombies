using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

/// <summary>タイトルの移動アニメーションを行うクラス </summary>
public class TitleBaseMove : MonoBehaviour
{
    /// <summary>α値の最大値</summary>
    const float MAX_FADE_VALUE = 1f;

    [SerializeField, Header("移動ルート(位置)"), Tooltip("順番に移動していきます")]
    Transform[] _movePositions = default;
    [SerializeField, Header("一周にかかる時間")]
    float _moveDuration = 0f;
    [SerializeField, Header("フェードにかかる時間")]
    float _fadeDuration = 0f;
    [SerializeField, Tooltip("ホーム画面遷移時に使用するパネル")]
    Image _transitionPanel = default;
    [SerializeField, Tooltip("ペラペラベル")]
    Image _titleLabel = default;
    [SerializeField, Tooltip("シェーダで動かしているラベル")]
    SpriteRenderer _square = default;
    [SerializeField, Tooltip("シャボン玉スポナー")]
    ParticleSystem _bubbleSpawner = default;

    void Start()
    {
        var routes = Array.ConvertAll(_movePositions, t => t.position);     //移動させる
        transform.DOPath(routes, _moveDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }


    /// <summary>ホーム画面へ遷移する </summary>
    public void HomeTransition(Action action = null)
    {
        _titleLabel.enabled = true;
        _square.enabled = false;
        _bubbleSpawner.gameObject.SetActive(false);

        _transitionPanel.DOFade(MAX_FADE_VALUE, _fadeDuration)
            .OnComplete(() => 
            {
                DOVirtual.DelayedCall(1f, () => 
                {
                    action?.Invoke();
                    gameObject.SetActive(false);
                });
            });
    }
}
