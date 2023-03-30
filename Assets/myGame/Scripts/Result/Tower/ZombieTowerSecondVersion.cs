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
            Debug.Log("�e�X�g�J�n");
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
            (int)_towerRectTransform.rect.width / (int)_zombieRect.sizeDelta.x; // ���ɐ�������]���r�̐�

        float oneLineHeight = _zombieRect.sizeDelta.y - _reduceDiffY; // ��s�ӂ�̍���
        // �|�������������[�v����B
        var generateCounter = 0;

        // �����ʒu(x����)�p�̒l
        float generatePosX;
        while (generateCounter < killScoreValue)
        {
            for (LoopInitialization(oneLineHeight); _generateCounter < oneLineGenerateValue + 1 && generateCounter < killScoreValue; _generateCounter++, generateCounter++)// ������ɕK�v�Ȑ���������
            {
                generatePosX =
                    _generateCounter * _zombieRect.rect.width +
                    _towerRectTransform.position.x - _towerRectTransform.sizeDelta.x / 2f; // �����ʒu(x����)���擾
                Debug.Log($"�����ʒux��{generatePosX}�ł�");
                // ��������B
                var zombie = ZombieGeneration(generatePosX);
                var rt = zombie.GetComponent<RectTransform>();
                rt.localPosition =
                    new Vector3(
                        rt.localPosition.x - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        rt.localPosition.y - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        0f);
                // z����]�������_���ɐݒ肷��
                var angleZ = Random.Range(0, 360);
                rt.Rotate(0f, 0f, angleZ);
                // �������̃A�j���[�V���������s����B
                ZombieAnimation(zombie.transform, _targetPosY);
                // �w�莞�ԑ҂B
                yield return new WaitForSeconds(generateInterval);
            }
            for (LoopInitialization(oneLineHeight);
                _generateCounter < oneLineGenerateValue && generateCounter < killScoreValue;
                _generateCounter++, generateCounter++) // ���ɕK�v�Ȑ���������
            {
                generatePosX =
                    _generateCounter * _zombieRect.rect.width +
                    _zombieRect.sizeDelta.x / 2f +
                    _towerRectTransform.position.x - _towerRectTransform.sizeDelta.x / 2f; // �����ʒu(x����)���擾
                Debug.Log($"�����ʒux��{generatePosX}�ł�");
                // ��������B
                var zombie = ZombieGeneration(generatePosX);
                // �ʒu�A��]�̒������s��
                // x,y���W�ɏ��������V�т���������
                var rt = zombie.GetComponent<RectTransform>();
                rt.localPosition =
                    new Vector3(
                        rt.localPosition.x - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        rt.localPosition.y - Random.Range(_randomPosDiffMin, _randomPosDiffMax),
                        0f);
                // z����]�������_���ɐݒ肷��
                var angleZ = Random.Range(0, 360);
                rt.Rotate(0f, 0f, angleZ);
                // �������̃A�j���[�V���������s����B
                ZombieAnimation(zombie.transform, _targetPosY);
                // �w�莞�ԑ҂B
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
        _generateCounter = 0; // for���Ŏg�p����l��������
        _targetPosY += oneLineHeight; // �ڕW�n�_(y���W)���X�V
        if (_targetPosY > Camera.main.transform.position.y) // �ڕW�n�_�̍������J������荂����΃J�����̈ʒu���X�V����
        {
            // �A�j���[�V�����Đ����ł���΃L������B
            _cameraAnim?.Kill();
            // �A�j���[�V�������Đ�����B
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
