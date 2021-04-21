using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
abstract public class Chunk : MonoBehaviour
{
    public AnimationCurve expandCurve;

    protected CanvasGroup faderCanvasGroup;
    protected bool settingUp = false;
    protected ScrollRect scrollRect;
    protected IEnumerator FadeIn(float fadeDuration)
    {
        settingUp = true;

        if(!faderCanvasGroup)
            faderCanvasGroup = GetComponent<CanvasGroup>();

        faderCanvasGroup.alpha = 0;

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - 1) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, 1))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, 1,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        settingUp = false;
    }

    public bool IsSettingUp()
    {
        return settingUp;
    }

    
}
