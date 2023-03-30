using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBGMController : MonoBehaviour
{
    [SerializeField]
    private AudioClip _bgm = default;
    void Start()
    {
        var audio = this.gameObject.AddComponent<AudioSource>();
        audio.clip = _bgm;
        audio.Play();
    }
}
