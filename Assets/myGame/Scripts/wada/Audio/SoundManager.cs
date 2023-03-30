using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public enum SE_Type
    {
        //　登録無料
        A = 0,
        B = 1,
        C = 2,
    }
    public enum BGM_Type
    {
        //　登録無料
        X = 0,
        Y = 1,
        Z = 2,
        SILENCE = 99,  // 無音
    }


    private static AudioSource _SoundEfectSource;
    private static AudioSource _BGMSource;

    private static SoundManager instanceSound;
    public static SoundManager InstanceSound 
    {
        get
        {
            if (instanceSound == null)
            {
                var soundObj = new GameObject("SoundManagerObj");
                instanceSound = soundObj.AddComponent<SoundManager>();
                _SoundEfectSource = soundObj.AddComponent<AudioSource>();

                var bgmObj = new GameObject("BGMObj");
                _BGMSource = bgmObj.AddComponent<AudioSource>();
                bgmObj.transform.parent = soundObj.transform;

                DontDestroyOnLoad(soundObj);
            }
            return instanceSound;
        }
    }



    public AudioClip GetSoundEffectList(int num)
    {
        var list = Resources.Load<SoundID>("Prefab/SoundList");
        return list.SE_Clips[num];
    }

    public AudioClip GetBGMList(int num)
    {
        var list = Resources.Load<SoundID>("Prefab/SoundList");
        return list.BGM_Clips[num];
    }


    //////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///　SEを鳴らす関数
    /// </summary>
    public void PlayAudioClip(SE_Type audioClip)
    {

        _SoundEfectSource.Stop();
        _SoundEfectSource.PlayOneShot(GetSoundEffectList((int)audioClip));

    }

    /// <summary>
    ///　BGMを鳴らすオーバーロード
    /// </summary>
    public void PlayAudioClip(BGM_Type audioClip)
    {
        _BGMSource.Stop();
        _BGMSource.PlayOneShot(GetBGMList((int)audioClip));
    }

    /// <summary>
    /// SEを指定の発生源で鳴らすおーばーろーど
    /// </summary>
    /// <param name="audioClip">流したい効果音</param>
    /// <param name="audioSource">音の発生源</param>
    public void PlayAudioClip(SE_Type audioClip, AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(GetSoundEffectList((int)audioClip));
    }

    /// <summary>
    /// BGMを指定の発生源で鳴らすおーばーろーど
    /// </summary>
    /// <param name="audioClip">流したい効果音</param>
    /// <param name="audioSource">音の発生源</param>
    public void PlayAudioClip(BGM_Type audioClip, AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(GetBGMList((int)audioClip));
    }

    public void SEVolumeSeT(float vol)
    {
        if(_SoundEfectSource == null)
        {
            Debug.LogError("SEのオーディオソースがないのだbyずんだもん");
            return;
        }
        _SoundEfectSource.volume = vol;
    }
    public void BGMVolumeSeT(float vol)
    {
        if (_BGMSource == null)
        {
            Debug.LogError("BGMのオーディオソースがないのだbyずんだもん");
            return;
        }
        _BGMSource.volume = vol;
    }
}