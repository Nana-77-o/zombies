using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

/// <summary>�^�C�g���̈ړ��A�j���[�V�������s���N���X </summary>
public class TitleBaseMove : MonoBehaviour
{
    /// <summary>���l�̍ő�l</summary>
    const float MAX_FADE_VALUE = 1f;

    [SerializeField, Header("�ړ����[�g(�ʒu)"), Tooltip("���ԂɈړ����Ă����܂�")]
    Transform[] _movePositions = default;
    [SerializeField, Header("����ɂ����鎞��")]
    float _moveDuration = 0f;
    [SerializeField, Header("�t�F�[�h�ɂ����鎞��")]
    float _fadeDuration = 0f;
    [SerializeField, Tooltip("�z�[����ʑJ�ڎ��Ɏg�p����p�l��")]
    Image _transitionPanel = default;
    [SerializeField, Tooltip("�y���y���x��")]
    Image _titleLabel = default;
    [SerializeField, Tooltip("�V�F�[�_�œ������Ă��郉�x��")]
    SpriteRenderer _square = default;
    [SerializeField, Tooltip("�V���{���ʃX�|�i�[")]
    ParticleSystem _bubbleSpawner = default;

    void Start()
    {
        var routes = Array.ConvertAll(_movePositions, t => t.position);     //�ړ�������
        transform.DOPath(routes, _moveDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }


    /// <summary>�z�[����ʂ֑J�ڂ��� </summary>
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
