using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
abstract public class Chunk : MonoBehaviour
{
    public WoodblockGameData data;
    public AnimationCurve expandCurve;

    protected CanvasGroup faderCanvasGroup;
    protected bool settingUp = false;
    protected ScrollRect scrollRect;

    abstract public void RefreshSettings();

    protected IEnumerator FadeIn()
    {
        settingUp = true;

        if(!faderCanvasGroup)
            faderCanvasGroup = GetComponent<CanvasGroup>();

        faderCanvasGroup.alpha = 0;

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - 1) / data.storyFadeDuration;

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
