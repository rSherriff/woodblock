using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(CanvasGroup))]
public class ImageChunk : Chunk
{
    private RawImage image;
    private LayoutElement layout;
    private float targetHeight;

    public void SetupImage(ScrollRect containerScrollRect, string img, int defaultLandscapeHeight, int defaultPortraitHeight,float fadeDuration, float imageSpeed, float fadeDelay, int desiredWidth = 0, int desiredHeight = 0)
    {
        settingUp = true;

        image = GetComponent<RawImage>();
        image.texture = FindObjectOfType<StoryImageManager>().GetImage(img);

        faderCanvasGroup = GetComponent<CanvasGroup>();
        faderCanvasGroup.alpha = 0;

        layout = GetComponent<LayoutElement>();
        layout.preferredHeight = 0;

        scrollRect = containerScrollRect;

        if (desiredWidth != 0 && desiredHeight != 0)
        {
            layout.preferredWidth = desiredWidth;
            targetHeight = desiredHeight;
        }
        else
        {
            float ratio = (float)image.texture.width / (float)image.texture.height;

            if (ratio > 1)
            {
                targetHeight = defaultPortraitHeight;
            }
            else
            {
                targetHeight = defaultLandscapeHeight;
            }
            layout.preferredWidth = targetHeight * ratio;
        }


        StartCoroutine(Expand(fadeDuration, imageSpeed, fadeDelay));
    }
    private IEnumerator Expand(float fadeDuration, float imageSpeed, float fadeDelay)
    {
        settingUp = true;

        while (!Mathf.Approximately(targetHeight, layout.preferredHeight))
        {
            layout.preferredHeight = Mathf.MoveTowards(layout.preferredHeight, targetHeight, imageSpeed * Time.deltaTime);

            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            scrollRect.verticalNormalizedPosition = 0;

            yield return null;
        }

        StartCoroutine(FadeIn(fadeDuration));

        yield return new WaitForSeconds(fadeDelay);

        settingUp = false;

        yield break;
    }
}
