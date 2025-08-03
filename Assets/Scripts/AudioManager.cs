using System;
using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _sfxSource;

   public AudioClip Ingridient;

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}