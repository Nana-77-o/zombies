using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class ZombieTowerThirdVersion : MonoBehaviour
{
    [Header("���s�ɕK�v�Ȓl�Q")]
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
    [Tooltip("x���X�P�[�������p")]
    [SerializeField]
    private float _scaleAdjustmentX = 1f;
    [Tooltip("y���X�P�[�������p")]
    [SerializeField]
    private float _scaleAdjustmentY = 1f;
    [Tooltip("�ő卂��")]
    [SerializeField]
    private float _maxHeight = default;
    [Tooltip("�Œ�ӂ̒���")]
    [SerializeField]
    private float _maxWidth = 1f;
    [Tooltip("���̗v�f�̒l�܂�,���݂̃C���f�b�N�X+1�̐��ɂ�1�� �s���~�b�h�ɐςރC���X�^���X�𐶐�����B")]
    [SerializeField]
    private int[] _deceptionBorderline = default;
    [SerializeField]
    private ResultImmutableData _immutableData = default;

    [Header("�����p�̒l")]
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
    [Header("�e�X�g�p�ɃC���X�y�N�^�E�B���h�E�ɕ\���������l")]
    private float _testValue;
    
    private void Start()
    {
        StartCoroutine(Play(() => _stringGroup.SetActive(true)));
    }

    /// <summary> ���o�̍Đ����� </summary>
    /// <param name = "onComplete" > �������Ɏ��s���������� </ param >
    public IEnumerator Play(System.Action onComplete = default)
    {
        int generatedNumber = 0; // ������������\���l
        int killNumber = _immutableData.GetScoreValue(ScoreType.Kill, 0);
        float startPosY = _cameraTarget.position.y;
        // �ő�񐔂������[�v���� // �������̐����[�v����
        for (int currentLineNum = 0; currentLineNum < GetMaxLineNumber() && generatedNumber < killNumber; currentLineNum++)
        {
            UpdateCameraPosUp(currentLineNum);
            // "0"����"���ɐ�������ő吔"�܂ł̐����l�� �V���b�t�����ꂽ�z����擾����B
            // �� : ����3�̐�������ꍇ {0, 2, 1}, ����5�̐�������ꍇ {4, 3, 0, 1, 2}
            // Mathf.Sqrt(GetMaxLineNumber())�Ȃ����̂悤�ȓ���ɂȂ�̂��悭�킩���Ă��Ȃ��B���܂��܂��܂����삵���B�����̕K�v�L��B
            int[] shufflePosX = ArrayShuffle(GetSetupArray(GetOneLineGenerateNumber(currentLineNum), currentLineNum / (int)Mathf.Round(Mathf.Sqrt(GetMaxLineNumber()))));
            // �e��Ƀ]���r�𐶐����� // �������̐����[�v����
            for (int generateNumber = 0; generateNumber < shufflePosX.Length && generatedNumber < killNumber; generateNumber++)
            {
                // �]���r�̐������W�̎擾����
                var generatePos = new Vector3
                        (
                            GetGeneratePosX(shufflePosX[generateNumber], true),
                            GetGeneratePosY(),
                            0f
                        );
                // �]���r�̐�������
                var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);
                generatedNumber++; // �������������J�E���g����B
                // ���W�̒���
                var localPos = zombie.transform.localPosition;
                localPos.z = 0f;
                localPos.x += _generateOffset.x;
                zombie.transform.localPosition = localPos;
                // �����_����z����]�̐ݒ�
                zombie.GetComponent<RectTransform>().
                    Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
                // �����A�j���[�V�����̍Đ�
                zombie.transform.
                    DOMoveY(GetTargetPosYRandom(currentLineNum), _zombieFallAnimDurationTime).
                    SetEase(_zombieFallAnimEase);
                // �ҋ@
                yield return new WaitForSeconds(_generateInterval);
            }
        }
        yield return new WaitForSeconds(_cameraTopWaitTime);
        // �~�����o���Đ�
        UpdateCameraPosDown(startPosY, onComplete);
    }
    #region Geter
    /// <summary> �w��̗�̒�ӂ̒������擾���� </summary>
    /// <param name="targetLine"> �Ώۂ̗�i��Ԓꂪ0�Ƃ��ăJ�E���g�A�b�v����z��ō쐻�������\�b�h�j </param>
    private float GetBaseLength(int targetLine)
    {
        return (((float)GetMaxLineNumber() - (float)targetLine) / (float)GetMaxLineNumber()) * _maxWidth;
    }
    /// <summary> 1��ɐ������鐔 </summary>
    /// <param name="targetLine"> �Ώۂ̗�i��Ԓꂪ0�Ƃ��ăJ�E���g�A�b�v����z��ō쐻�������\�b�h�j </param>
    private int GetOneLineGenerateNumber(int targetLine)
    {
        return (int)(GetBaseLength(targetLine) / GetZombieWidth());
    }
    /// <summary> ��̍ő吔 </summary>
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
    /// <summary> �J�����̏㏸���� </summary>
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
    /// <summary> �J�����̍~������ </summary>
    private void UpdateCameraPosDown(float standardPosY, System.Action onComplete)
    {
        _cameraTarget.
            DOMoveY(standardPosY, _cameraFallAnimDurationTime).
            SetEase(_cameraFallAnimEase).
            OnComplete(() => onComplete?.Invoke());
    }
    // �z��̗v�f���V���b�t��������@ https://dobon.net/vb/dotnet/programing/arrayshuffle.html
    // �o�u���\�[�g�̂悤�ɕ��ѕς��邪�A�������w�肹�������_���ɕ��ѕς���B
    /// <summary> �z��̗v�f���V���b�t�����߂�l�Ƃ��ĕԂ��B </summary>
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
    /// <summary> �v�f���Z�b�g�A�b�v�ς݂̔z����擾���� </summary>
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