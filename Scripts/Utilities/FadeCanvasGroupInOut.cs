using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasGroupInOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public AnimationCurve fadeCurve;
    public bool playOnAwake;

    void OnEnable()
    {
        if(playOnAwake)
        {
            StartCoroutine(Fade());
        }
    }

    public void StartFade()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float fadeTime = 0;

        while(fadeTime < fadeCurve.keys[fadeCurve.length - 1].time)
        {
            canvasGroup.alpha = fadeCurve.Evaluate(fadeTime);
            fadeTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = fadeCurve.Evaluate(fadeTime);
    }
}
