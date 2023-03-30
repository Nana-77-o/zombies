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
    [Header("���s�ɕK�v�Ȓl�Q")]
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
    [Tooltip("���l�����̐e�I�u�W�F�N�g"), SerializeField]
    private Transform _villagersParentObj = default;
    [Tooltip("�v���C���[�̃A�j���[�^�["), SerializeField]
    private Animator _playerAnimator = default;

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
    [Tooltip("������{�^����������Ă��鎞�̉����x"), SerializeField, Range(1f, 10f)]
    private float _pyramidSpeedAcceleration = 1f;

    /// <summary> ��������X���W�̃X�^�b�N </summary>
    private Stack<int> _generatePosXStack = new Stack<int>();
    /// <summary> ���̍s�ɂ����������邩�L�����Ă����X�^�b�N </summary>
    private Stack<int> _generateNumberStack = new Stack<int>();

    private void Start()
    {
        // �\�����鑺�l�̐��𒲐�
        Debug.Log($"���������l�̐�{_immutableData.GetScoreValue(ScoreType.Help, 12)}");
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
        // ���o�Đ�����
        // ���o�̗��� : �s���~�b�h���o�����l�_���X���o���]�����o�Đ�
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
        // �����ς݂̐���\���l
        int generatedNumber = 0;
        // ��������]���r�̐����擾
        int generateNumber = AdjustKillValue(_immutableData.GetScoreValue(ScoreType.Kill, 0));
        // �㏸���o��A�J�n�ʒu�܂ō~������̂ŁA�J�n�ʒu���L�����Ă���
        float startPosY = _cameraTarget.position.y;
        // ��������ʒu���X�^�b�N����B
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
        // �J�E���^�[�ϐ���������
        generatedNumber = 0;
        // ����������̐����J�E���g����ϐ�
        int generateLineCounter = 0;
        // �X�v���C�g���擾����
        var sprites = _immutableData.GetKillScoreShuffleSprites();
        // �X�^�b�N�����ʒu�Ɍ������ă]���r�𐶐�����
        for (int currentLineNum = 0; currentLineNum < GetMaxLineNumber() && generatedNumber < generateNumber; currentLineNum++)
        {
            generateLineCounter++;
            // �J�����̈ʒu���X�V����
            UpdateCameraPosUp(currentLineNum);
            // ���̍s�ɐ������鐔���擾����
            var maxGenerateNumber = _generateNumberStack.Pop();
            // �e�]���r�𐶐�����
            for (int generateNumberOfThisLine = 0; generateNumberOfThisLine < maxGenerateNumber && generatedNumber < generateNumber; generateNumberOfThisLine++)
            {
                // �]���r�̐������W�̎擾����
                var generatePos = new Vector3
                        (
                            GetGeneratePosX(_generatePosXStack.Pop(), true),
                            GetGeneratePosY(),
                            0f
                        );
                // �]���r�̐�������
                if (generatedNumber < sprites.Length) // �ꎞ�I�ȉ������
                {
                    GenerateZombie(generatePos, currentLineNum, sprites[generatedNumber]);
                }
                else
                {
                    Debug.Log($"������Index���w�肳��܂����B{generatedNumber}�͔͈͊O�ł��I");
                }
                // �����ς݂̃]���r���J�E���g����
                generatedNumber++;
                if (!_resultController.IsSkip) // �X�L�b�v���Ȃ��Ƃ��͑ҋ@����B
                {
                    // �ҋ@
                    yield return new WaitForSeconds(_generateInterval / _resultController.CurrentPlaySpeed);
                }
            }
        }

        // ���_�ɂĎw�莞�ԁA�J�����̈ʒu��ҋ@������B
        yield return new WaitForSeconds(_cameraTopWaitTime / _resultController.CurrentPlaySpeed);
        // �~�����o���Đ�
        UpdateCameraPosDown(startPosY, onComplete, (float)generateLineCounter / (float)GetMaxLineNumber());
    }
    #region Geter
    /// <summary> �w��̗�̒�ӂ̒������擾���� </summary>
    /// <param name="targetLine"> �Ώۂ̗�i��Ԓꂪ0�Ƃ��ăJ�E���g�A�b�v����z��ō쐻�������\�b�h�j </param>
    private float GetBaseLength(int targetLine)
    {
        return (((float)GetMaxLineNumber() - (float)targetLine) / (float)GetMaxLineNumber()) * _maxWidth;
    }
    /// <summary> 1�s�ɐ������鐔 </summary>
    /// <param name="targetLine"> �Ώۂ̗�i��Ԓꂪ0�Ƃ��ăJ�E���g�A�b�v����z��ō쐻�������\�b�h�j </param>
    private int GetOneLineGenerateNumber(int targetLine)
    {
        //            ��ӂ̒���               / �]���r��̂�����̕�
        return (int)(GetBaseLength(targetLine) / GetZombieWidth());
    }
    /// <summary> �����ł���s�̍ő吔 </summary>
    private int GetMaxLineNumber()
    {
        //            �ő卂��  / �]���r��̂�����̍���
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
    /// �w�肳�ꂽ�l�𒲐����A�]���r�ςݏグ���o�Ɏg�p����l��Ԃ����\�b�h�B
    /// </summary>
    private int AdjustKillValue(int targetValue)
    {
        int result = 0; // ���ʗp�ϐ�
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
    /// <summary> �J�����̏㏸���� </summary>
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
    /// <summary> �J�����̍~������ </summary>
    private void UpdateCameraPosDown(float standardPosY, System.Action onComplete, float timeRate)
    {
        _cameraTarget.
            DOMoveY(standardPosY, _cameraFallAnimDurationTime * timeRate).
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
    /// <summary> �]���r�̐������� </summary>
    private void GenerateZombie(Vector3 generatePos, int targetLine)
    {
        if (!_resultController.IsSkip) // �X�L�b�v���Ȃ��Ƃ��͑ҋ@����B
        {
            // �]���r�̐�������
            var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);

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
    /// <summary> �]���r�̐������� </summary>
    private void GenerateZombie(Vector3 generatePos, int targetLine, Sprite sprite)
    {
        if (!_resultController.IsSkip) // �X�L�b�v���Ȃ��Ƃ��͑ҋ@����B
        {
            // �]���r�̐�������
            var zombie = Instantiate(_zombiePrefab, generatePos, Quaternion.identity, _zombieParentCanvas);

            // �X�v���C�g�̊��蓖��
            zombie.GetComponent<Image>().sprite = sprite;
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
                DOMoveY(GetTargetPosYRandom(targetLine), _zombieFallAnimDurationTime / _resultController.CurrentPlaySpeed).
                SetEase(_zombieFallAnimEase);
        }
        else
        {
            var zombie = Instantiate(_zombiePrefab, new Vector3(generatePos.x, GetTargetPosYRandom(targetLine),
                generatePos.z), Quaternion.identity, _zombieParentCanvas);

            // �X�v���C�g�̊��蓖��
            zombie.GetComponent<Image>().sprite = sprite;
            // ���W�̒���
            var localPos = zombie.transform.localPosition;
            localPos.z = 0f;
            localPos.x += _generateOffset.x;
            zombie.transform.localPosition = localPos;
            // �����_����z����]�̐ݒ�
            zombie.GetComponent<RectTransform>().
                Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
        }
    }
    /// <summary>
    /// ���l�_���X���Đ�����
    /// </summary>
    /// <param name="onComplete"> �_���X�������Ɏ��s����A�N�V���� </param>
    private IEnumerator VillagerAnimStart(System.Action onComplete)
    {
        if (!_resultController.IsSkip)
        {
            // �v���C���[�̃_���X���J�n����
            _playerAnimator.SetBool("DanceStart", true);
            // �A�j���[�V�����I�����o�p�R���|�[�l���g���擾����B
            var endDetection = _playerAnimator.GetComponent<VillagerAnimEndDetection>();
            for (int i = 0; i < _villagersParentObj.childCount; i++)
            {
                // ���������������[�v����
                if (i < _immutableData.GetScoreValue(ScoreType.Help, 12))
                {
                    // ���l�̃_���X�A�j���[�V�������J�n����
                    _villagersParentObj.GetChild(i).GetComponent<Animator>().SetBool("DanceStart", true);
                    if (endDetection == null) // �v���C���[���A�j���[�V���������p�R���|�[�l���g���������Ă��Ȃ��\����z��B
                                              // �������Ă��Ȃ��ꍇ�A���l����擾����B
                    {
                        endDetection = _villagersParentObj.GetChild(i).GetComponent<VillagerAnimEndDetection>();
                    }
                }
                else
                {
                    break;
                }
            }
            // �A�j���[�V��������������܂őҋ@����
            // ������ false �̎�, �ҋ@����B
            yield return new WaitUntil(() => endDetection.IsAnimEnd);
        }

        onComplete?.Invoke();
    }
    /// <summary>
    /// �_���X�A�j���[�V�����X�s�[�h��ύX����
    /// </summary>
    private void ChangeDanceAnimSpeed(float animSpeed)
    {
        _playerAnimator.SetFloat("DanceSpeed", animSpeed);

        for (int i = 0; i < _villagersParentObj.childCount; i++)
        {
            // ���������������[�v����
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