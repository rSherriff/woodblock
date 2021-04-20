using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AssetBundle clapAudioBundle;
    AssetBundle ambientAudioBundle;

    void Start()
    {
        /*clapAudioBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "clapaudio"));
        if (clapAudioBundle == null)
        {
            Debug.LogError("Failed to load clapAudioBundle AssetBundle!");
            return;
        }

        ambientAudioBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "ambientaudio"));
        if (ambientAudioBundle == null)
        {
            Debug.LogError("Failed to load ambientAudioBundle AssetBundle!");
            return;
        }*/
    }

    public AudioClip GetClapAudio(string name)
    {
        GameManager manager = FindObjectOfType<GameManager>();
        return Resources.Load<AudioClip>("Audio/Claps/" + name);
    }

    public AudioClip GetAmbientAudio(string name)
    {
        GameManager manager = FindObjectOfType<GameManager>();
        return Resources.Load<AudioClip>("Audio/Ambient/" + name);
    }
}
