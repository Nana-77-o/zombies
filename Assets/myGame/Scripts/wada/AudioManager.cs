using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BGMs
{
    �v���CBGM,
    ���U���gBGM,
    ���[�hBGM,
    �Q�[���I�[�o�[BGM,

    ����Sound,
    �L�����Z��Sound,
    �i�C�tSound,
    �����XSound,
    �V���{����Sound,
    �n��Sound,
    �]���rSound,
    �]���r���H��Sound,
    �v���C���[�_���[�WSound,
    �v���C���[�ړ�Sound,
    �I�u�W�F�N�g�j��Sound,
    �]���r�^���[�ςݏグSound,
    ���U���gSound,
    �]���r�_���[�WSound,
    ����`�F���WSound,

}


public class AudioManager : MonoBehaviour
{

    [Tooltip("�v���CBGM"), SerializeField]
    AudioClip _playBGM;
    [Tooltip("���U���gBGM"), SerializeField]
    AudioClip _resultBGM;
    [Tooltip("���[�hBGM"), SerializeField]
    AudioClip _loadBGM;
    [Tooltip("�Q�[���I�[�o�[BGM"), SerializeField]
    AudioClip _gameOverBGM;

    [Tooltip("����Sound"), SerializeField]
    AudioClip _decisionSound;
    [Tooltip("�L�����Z��Sound"), SerializeField]
    AudioClip _cancelSoud;
    [Tooltip("�i�C�tSound"), SerializeField]
    AudioClip _knifeSound;
    [Tooltip("�����XSound"), SerializeField]
    AudioClip _lanceSound;
    [Tooltip("�V���{����Sound"), SerializeField]
    AudioClip _bubbleSound;
    [Tooltip("�n��Sound"), SerializeField]
    AudioClip _mineSound;
    [Tooltip("�]���rSound"), SerializeField]
    AudioClip _zombieSound;
    [Tooltip("�]���r���H��Sound"), SerializeField]
    AudioClip _zombiecannibalSound;
    [Tooltip("�v���C���[�_���[�WSound"), SerializeField]
    AudioClip _playerDamageSound;
    [Tooltip("�v���C���[�ړ�Sound"), SerializeField]
    AudioClip _playerMoveSound;
    [Tooltip("�I�u�W�F�N�g�j��Sound"), SerializeField]
    AudioClip _objectDestructionSound;
    [Tooltip("�]���r�^���[�ςݏグSound"), SerializeField]
    AudioClip _zombiePileUpSound;
    [Tooltip("���U���gSound"), SerializeField]
    AudioClip _resultSound;
    [Tooltip("�]���r�_���[�WSound"), SerializeField]
    AudioClip _zombieDamageSound;
    [Tooltip("����`�F���WSound"), SerializeField]
    AudioClip _weaponChangeSound;


    [Tooltip("BGM�̃I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _bgmAudioSource;
    [Tooltip("�v���C���[����o�鉹�̃I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _audioSource;

    [Tooltip("BGM�̉��ʃX���C�_�["), SerializeField]
    Slider _bgmSlider;
    [Tooltip("BGM�̉��ʃX���C�_�["), SerializeField]
    Slider _soundSlider;

    /// <summary>
    /// �I�[�f�B�I�}�l�[�W���[���特��炷�ۂɌĂяo���֐�
    /// </summary>
    /// <param name="bgms">�炵��������Enum</param>
    public void PlayMyAudioSource(BGMs bgms)
    {
        switch (bgms)
        {
            case BGMs.�v���CBGM:
                _bgmAudioSource.PlayOneShot(_playBGM);
                break;

            case BGMs.���U���gBGM:
                _bgmAudioSource.PlayOneShot(_resultBGM);
                break;

            case BGMs.���[�hBGM:
                _bgmAudioSource.PlayOneShot(_loadBGM);
                break;

            case BGMs.�Q�[���I�[�o�[BGM:
                _bgmAudioSource.PlayOneShot(_gameOverBGM);
                break;

            default:
                Debug.Log("�����˂���");
                break;
        }
    }

    /// <summary>
    /// �ʂŉ���炷�ۂɌĂяo���֐�
    /// </summary>
    /// <param name="audioSource">�Ăяo�����̃I�[�f�B�I�\�[�X</param>
    /// <param name="bgms">�炵��������Enum</param>
    public void PlayOtherAudioSource(AudioSource audioSource, BGMs sounds)
    {
        switch (sounds)
        {
            case BGMs.����Sound:
                audioSource.PlayOneShot(_decisionSound);
                break;

            case BGMs.�L�����Z��Sound:
                audioSource.PlayOneShot(_cancelSoud);
                break;

            case BGMs.�i�C�tSound:
                audioSource.PlayOneShot(_knifeSound);
                break;

            case BGMs.�����XSound:
                audioSource.PlayOneShot(_lanceSound);
                break;
            case BGMs.�V���{����Sound:
                audioSource.PlayOneShot(_bubbleSound);
                break;

            case BGMs.�n��Sound:
                audioSource.PlayOneShot(_mineSound);
                break;

            case BGMs.�]���rSound:
                audioSource.PlayOneShot(_zombieSound);
                break;

            case BGMs.�]���r���H��Sound:
                audioSource.PlayOneShot(_zombiecannibalSound);
                break;
            case BGMs.�v���C���[�_���[�WSound:
                audioSource.PlayOneShot(_playerDamageSound);
                break;

            case BGMs.�v���C���[�ړ�Sound:
                audioSource.PlayOneShot(_playerMoveSound);
                break;

            case BGMs.�I�u�W�F�N�g�j��Sound:
                audioSource.PlayOneShot(_objectDestructionSound);
                break;

            case BGMs.�]���r�^���[�ςݏグSound:
                audioSource.PlayOneShot(_zombiePileUpSound);
                break;
            case BGMs.���U���gSound:
                audioSource.PlayOneShot(_resultSound);
                break;

            case BGMs.�]���r�_���[�WSound:
                audioSource.PlayOneShot(_zombieDamageSound);
                break;

            case BGMs.����`�F���WSound:
                audioSource.PlayOneShot(_weaponChangeSound);
                break;

            default:
                Debug.Log("�����˂���");
                break;
        }
    }

    private void Start()
    {
        //_bgmSlider.value = _bgmAudioSource.volume;
        //_soundSlider.value = _audioSource.volume;
    }

    private void FixedUpdate()
    {
        //_bgmAudioSource.volume = _bgmSlider.value;
        //_audioSource.volume = _soundSlider.value;
    }
}
