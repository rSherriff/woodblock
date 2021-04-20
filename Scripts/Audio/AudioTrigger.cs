using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    public void TriggerAudio()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (audioSource.isPlaying)
            audioSource.Stop();
        else
            audioSource.Play();
    }
}
