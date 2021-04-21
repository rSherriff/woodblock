using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
[RequireComponent(typeof(Image))]
public class TextChunk : Chunk
{
    public TextMeshProUGUI textMesh;

    private LayoutElement layout;
    private List<StoryLine> lines;

    private void Start()
    {
        lines = new List<StoryLine>();
    }

    public void SetupText(ScrollRect containerScrollRect, List<StoryLine> linesIn, bool shouldFade, float speed, int scrollCutoff, float fadeDuration)
    {
        StartCoroutine(Setup(containerScrollRect, linesIn, shouldFade, speed, scrollCutoff, fadeDuration));
    }

    public void SetupText(ScrollRect containerScrollRect, StoryLine lineIn, bool shouldFade, float speed, int scrollCutoff, float fadeDuration)
    {
        List<StoryLine> lines = new List<StoryLine>();
        lines.Add(lineIn);

        StartCoroutine(Setup(containerScrollRect, lines, shouldFade, speed, scrollCutoff, fadeDuration));
    }

    public IEnumerator Setup(ScrollRect containerScrollRect, List<StoryLine> linesIn, bool shouldFade, float speed, int scrollCutoff, float fadeDuration)
    {
        settingUp = true;

        if (!faderCanvasGroup)
            faderCanvasGroup = GetComponent<CanvasGroup>();

        faderCanvasGroup.alpha = 0;
        textMesh.text = "";
        GetComponent<LayoutElement>().preferredHeight = 0;
        yield return new WaitForSeconds(0.0f);

        lines = linesIn;

        textMesh.transform.SetParent(this.transform, false);

        scrollRect = containerScrollRect;

        for (int i = 0; i < lines.Count; i++)
        {
            string text = lines[i].text.TrimStart(' ','\r', '\n');

            if (text.Length <= 0)
                continue;

            bool skipLines = false;
            if (lines[i].tags != null && lines[i].tags.Count > 0)
                foreach (string t in lines[i].tags)
                {
                    if (t.StartsWith("INTERACTABLE"))
                    {
                        skipLines = true;
                    }
                    else if (t.StartsWith("END_INTERACTABLE"))
                    {
                        skipLines = true;
                    }
                }

            if (!skipLines)
                textMesh.text += text;
        }

        GetComponent<LayoutElement>().preferredHeight = textMesh.preferredHeight;

        if (!shouldFade)
            StartCoroutine(Expand(speed, scrollCutoff, fadeDuration));
        else
            StartCoroutine(FadeIn(fadeDuration));
    }

    private IEnumerator Expand(float speed, int scrollCutoff, float fadeDuration)
    {
        settingUp = true;

        layout = GetComponent<LayoutElement>();

        float desiredHeight = layout.preferredHeight;
        layout.preferredHeight = 0;
        while (!Mathf.Approximately(desiredHeight, layout.preferredHeight) && layout.preferredHeight < scrollCutoff)
        {
            layout.preferredHeight = Mathf.MoveTowards(layout.preferredHeight, desiredHeight, speed * Time.deltaTime);

            
            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            scrollRect.verticalNormalizedPosition = 0;

            yield return null;
        }

        if (!Mathf.Approximately(desiredHeight, layout.preferredHeight))
        {
            layout.preferredHeight = desiredHeight;
        }

        StartCoroutine(FadeIn(fadeDuration));

        settingUp = false;
    }

    public string GetText()
    {
        if (settingUp)
        {
            return "";
        }

        return textMesh.text;
    }
}
