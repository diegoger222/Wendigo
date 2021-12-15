using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public bool disableOnce = false;
    AudioSource audioSource;
    public AudioClip aud1;
    public AudioClip aud2;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound1()
    {
        audioSource.PlayOneShot(aud1, 1f);
    }

    public void PlaySound2()
    {
        audioSource.PlayOneShot(aud2, 1f);
    }

    public bool GetDisableOnce()
    {
        return disableOnce;
    }

    public void InvertDisableOnce()
    {
        disableOnce = !disableOnce;
    }
}
