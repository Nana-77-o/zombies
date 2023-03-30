using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BGMs
{
    プレイBGM,
    リザルトBGM,
    ロードBGM,
    ゲームオーバーBGM,

    決定Sound,
    キャンセルSound,
    ナイフSound,
    ランスSound,
    シャボン玉Sound,
    地雷Sound,
    ゾンビSound,
    ゾンビ共食いSound,
    プレイヤーダメージSound,
    プレイヤー移動Sound,
    オブジェクト破壊Sound,
    ゾンビタワー積み上げSound,
    リザルトSound,
    ゾンビダメージSound,
    武器チェンジSound,

}


public class AudioManager : MonoBehaviour
{

    [Tooltip("プレイBGM"), SerializeField]
    AudioClip _playBGM;
    [Tooltip("リザルトBGM"), SerializeField]
    AudioClip _resultBGM;
    [Tooltip("ロードBGM"), SerializeField]
    AudioClip _loadBGM;
    [Tooltip("ゲームオーバーBGM"), SerializeField]
    AudioClip _gameOverBGM;

    [Tooltip("決定Sound"), SerializeField]
    AudioClip _decisionSound;
    [Tooltip("キャンセルSound"), SerializeField]
    AudioClip _cancelSoud;
    [Tooltip("ナイフSound"), SerializeField]
    AudioClip _knifeSound;
    [Tooltip("ランスSound"), SerializeField]
    AudioClip _lanceSound;
    [Tooltip("シャボン玉Sound"), SerializeField]
    AudioClip _bubbleSound;
    [Tooltip("地雷Sound"), SerializeField]
    AudioClip _mineSound;
    [Tooltip("ゾンビSound"), SerializeField]
    AudioClip _zombieSound;
    [Tooltip("ゾンビ共食いSound"), SerializeField]
    AudioClip _zombiecannibalSound;
    [Tooltip("プレイヤーダメージSound"), SerializeField]
    AudioClip _playerDamageSound;
    [Tooltip("プレイヤー移動Sound"), SerializeField]
    AudioClip _playerMoveSound;
    [Tooltip("オブジェクト破壊Sound"), SerializeField]
    AudioClip _objectDestructionSound;
    [Tooltip("ゾンビタワー積み上げSound"), SerializeField]
    AudioClip _zombiePileUpSound;
    [Tooltip("リザルトSound"), SerializeField]
    AudioClip _resultSound;
    [Tooltip("ゾンビダメージSound"), SerializeField]
    AudioClip _zombieDamageSound;
    [Tooltip("武器チェンジSound"), SerializeField]
    AudioClip _weaponChangeSound;


    [Tooltip("BGMのオーディオソース"), SerializeField]
    AudioSource _bgmAudioSource;
    [Tooltip("プレイヤーから出る音のオーディオソース"), SerializeField]
    AudioSource _audioSource;

    [Tooltip("BGMの音量スライダー"), SerializeField]
    Slider _bgmSlider;
    [Tooltip("BGMの音量スライダー"), SerializeField]
    Slider _soundSlider;

    /// <summary>
    /// オーディオマネージャーから音を鳴らす際に呼び出す関数
    /// </summary>
    /// <param name="bgms">鳴らしたい音のEnum</param>
    public void PlayMyAudioSource(BGMs bgms)
    {
        switch (bgms)
        {
            case BGMs.プレイBGM:
                _bgmAudioSource.PlayOneShot(_playBGM);
                break;

            case BGMs.リザルトBGM:
                _bgmAudioSource.PlayOneShot(_resultBGM);
                break;

            case BGMs.ロードBGM:
                _bgmAudioSource.PlayOneShot(_loadBGM);
                break;

            case BGMs.ゲームオーバーBGM:
                _bgmAudioSource.PlayOneShot(_gameOverBGM);
                break;

            default:
                Debug.Log("音がねぇよ");
                break;
        }
    }

    /// <summary>
    /// 個別で音を鳴らす際に呼び出す関数
    /// </summary>
    /// <param name="audioSource">呼び出し元のオーディオソース</param>
    /// <param name="bgms">鳴らしたい音のEnum</param>
    public void PlayOtherAudioSource(AudioSource audioSource, BGMs sounds)
    {
        switch (sounds)
        {
            case BGMs.決定Sound:
                audioSource.PlayOneShot(_decisionSound);
                break;

            case BGMs.キャンセルSound:
                audioSource.PlayOneShot(_cancelSoud);
                break;

            case BGMs.ナイフSound:
                audioSource.PlayOneShot(_knifeSound);
                break;

            case BGMs.ランスSound:
                audioSource.PlayOneShot(_lanceSound);
                break;
            case BGMs.シャボン玉Sound:
                audioSource.PlayOneShot(_bubbleSound);
                break;

            case BGMs.地雷Sound:
                audioSource.PlayOneShot(_mineSound);
                break;

            case BGMs.ゾンビSound:
                audioSource.PlayOneShot(_zombieSound);
                break;

            case BGMs.ゾンビ共食いSound:
                audioSource.PlayOneShot(_zombiecannibalSound);
                break;
            case BGMs.プレイヤーダメージSound:
                audioSource.PlayOneShot(_playerDamageSound);
                break;

            case BGMs.プレイヤー移動Sound:
                audioSource.PlayOneShot(_playerMoveSound);
                break;

            case BGMs.オブジェクト破壊Sound:
                audioSource.PlayOneShot(_objectDestructionSound);
                break;

            case BGMs.ゾンビタワー積み上げSound:
                audioSource.PlayOneShot(_zombiePileUpSound);
                break;
            case BGMs.リザルトSound:
                audioSource.PlayOneShot(_resultSound);
                break;

            case BGMs.ゾンビダメージSound:
                audioSource.PlayOneShot(_zombieDamageSound);
                break;

            case BGMs.武器チェンジSound:
                audioSource.PlayOneShot(_weaponChangeSound);
                break;

            default:
                Debug.Log("音がねぇよ");
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
