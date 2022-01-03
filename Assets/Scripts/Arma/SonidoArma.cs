using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoArma : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip aud1;
    public AudioClip aud2;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundShot()
    {
        audioSource.PlayOneShot(aud1, 1f);
    }

    public void PlaySoundRecharge()
    {
        audioSource.PlayOneShot(aud2, 1f);
    }
}
