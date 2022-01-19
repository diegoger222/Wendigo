using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoJugador : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip aud1;
    public AudioClip aud2;
    public AudioClip aud3;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundDolor()
    {
        int valor = Random.Range(0, 2);
        if (valor == 0) { audioSource.PlayOneShot(aud1, 2f); }
        else { audioSource.PlayOneShot(aud2, 2f); }
    }

    public void PlaySoundMuerte()
    {
        audioSource.PlayOneShot(aud3, 2f);
    }

    //public void StopPlayAllSounds()
    //{
    //    audioSource.Stop();
    //}
}
