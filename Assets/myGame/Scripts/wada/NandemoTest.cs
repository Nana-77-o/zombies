using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NandemoTest : MonoBehaviour
{
    [SerializeField] SoundManager.SE_Type _audioClip;

    [SerializeField] SoundManager.BGM_Type _bgmClip;

    //[SerializeField] bool a;
    //public void AudioPlay()
    //{
    //    if(a)
    //    {
    //        SoundManager.InstanceSound.PlayAudioClip(_audioClip);
    //    }
    //    else
    //    {
    //        SoundManager.InstanceSound.PlayAudioClip(_bgmClip);
    //    }
    //}

    private void Start()
    {
        StartCoroutine(A());
    }

    IEnumerator A()
    {
        SoundManager.InstanceSound.PlayAudioClip(_audioClip);
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(B());
    }
    IEnumerator B()
    {
        SoundManager.InstanceSound.PlayAudioClip(_bgmClip);
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(A());
    }
}
