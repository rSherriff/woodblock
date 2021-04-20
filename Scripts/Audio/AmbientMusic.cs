using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;
    public float crossfadeDuration = 2.0f;
    string currentTrackName;
    bool isFading = false;

    private enum CURRENT_AUDIO
    {
        ONE, 
        TWO,
    }
    CURRENT_AUDIO currentAudio;

    public void Start()
    {
        StartCoroutine("CrossfadeOnAudioEnd");
        currentAudio = CURRENT_AUDIO.TWO;
    }

    public void StartNewTrack(string name)
    {
        if (currentTrackName == name) return;

        currentTrackName = name;
        Debug.Log("Changing Ambient track to " + name);

        StopAllCoroutines();
        StartCoroutine("CrossfadeOnAudioEnd");
        StartCoroutine("NewTrackRoutine");
    }

    private IEnumerator NewTrackRoutine()
    {
        AudioSource audioSource = GetCurrentAudioSource();
        AudioSource newAudioSource = GetUnusedAudioSource();

        StopCoroutine("FadeAudio");

        StartCoroutine(FadeAudio(audioSource, 0));

        newAudioSource.clip = FindObjectOfType<AudioManager>().GetAmbientAudio(currentTrackName);
        newAudioSource.Play();

        StartCoroutine(FadeAudio(newAudioSource, 1f));

        currentAudio = GetAudioEnum(newAudioSource);

        yield break;
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float finalVolume)
    {
        isFading = true;

        float fadeSpeed = Mathf.Abs(audioSource.volume - finalVolume) / crossfadeDuration;

        while (!Mathf.Approximately(audioSource.volume, finalVolume))
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, finalVolume,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
    }

    private IEnumerator CrossfadeOnAudioEnd()
    { 
        for (; ; )
        {
            AudioSource audioSource = GetCurrentAudioSource();

            //If we're less than three seconds before the end.
            if (audioSource.isPlaying && audioSource.clip.length - audioSource.time < 3.0f)
            {

                //Fade Out if we're not already fading
                if (!isFading)
                {
                    StartCoroutine(FadeAudio(audioSource, 0));

                    //Wait for the track to loop
                    yield return new WaitForSeconds(3.0f);

                    StartCoroutine(FadeAudio(audioSource, 0.15f));
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private AudioSource GetCurrentAudioSource()
    {
        switch (currentAudio)
        {
            case CURRENT_AUDIO.ONE:
                return audioSourceOne;
            case CURRENT_AUDIO.TWO:
                return audioSourceTwo;
            default:
                return audioSourceOne;
        }
    }

    private AudioSource GetUnusedAudioSource()
    {
        switch (currentAudio)
        {
            case CURRENT_AUDIO.ONE:
                return audioSourceTwo;
            case CURRENT_AUDIO.TWO:
                return audioSourceOne;
            default:
                return audioSourceTwo;
        }
    }

    CURRENT_AUDIO GetAudioEnum(AudioSource audioSource)
    {
        if (audioSource == audioSourceOne)
            return CURRENT_AUDIO.ONE;
        else if (audioSource == audioSourceTwo)
            return CURRENT_AUDIO.TWO;

        return CURRENT_AUDIO.ONE;
    }

    public string GetCurrentTrackName()
    {
        return currentTrackName;
    }
}
