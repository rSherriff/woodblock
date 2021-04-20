using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clap : MonoBehaviour
{
    AudioSource audioSource;
    bool setup = false;

    private void Update()
    {
        if(setup && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = FindObjectOfType<AudioManager>().GetClapAudio(clip);
        audioSource.Play();
        setup = true;

    }

    public void Setup(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        setup = true;

    }

}
