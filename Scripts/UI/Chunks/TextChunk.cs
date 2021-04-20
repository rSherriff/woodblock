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
    private Image backgroundImage;

    private void Start()
    {
        lines = new List<StoryLine>();
        backgroundImage = GetComponent<Image>();
        RefreshSettings();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            RefreshSettings();
        }
    }

    override public void RefreshSettings()
    {
        textMesh.font = data.font;
        textMesh.color = data.fontColor;
        textMesh.fontSize = data.fontSize;
        textMesh.lineSpacing = data.lineSpacing;
        textMesh.paragraphSpacing = data.paragraphSpacing;
        backgroundImage.color = data.textBackgroundColor;
    }

    public void SetupText(ScrollRect containerScrollRect, List<StoryLine> linesIn, bool shouldFade = false)
    {
        StartCoroutine(Setup(containerScrollRect, linesIn, shouldFade));
    }

    public void SetupText(ScrollRect containerScrollRect, StoryLine lineIn, bool shouldFade = false)
    {
        List<StoryLine> lines = new List<StoryLine>();
        lines.Add(lineIn);

        StartCoroutine(Setup(containerScrollRect, lines, shouldFade));
    }

    public IEnumerator Setup(ScrollRect containerScrollRect, List<StoryLine> linesIn, bool shouldFade = false)
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
            StartCoroutine(Expand());
        else
            StartCoroutine(FadeIn());
    }

    private IEnumerator Expand()
    {
        settingUp = true;

        layout = GetComponent<LayoutElement>();

        float desiredHeight = layout.preferredHeight;
        layout.preferredHeight = 0;
        while (!Mathf.Approximately(desiredHeight, layout.preferredHeight) && layout.preferredHeight < data.textBoxScrollCutoff)
        {
            layout.preferredHeight = Mathf.MoveTowards(layout.preferredHeight, desiredHeight, data.textSpeed * Time.deltaTime);

            
            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            scrollRect.verticalNormalizedPosition = 0;

            yield return null;
        }

        if (!Mathf.Approximately(desiredHeight, layout.preferredHeight))
        {
            layout.preferredHeight = desiredHeight;
        }

        StartCoroutine(FadeIn());

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
