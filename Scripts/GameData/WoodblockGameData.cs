using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "UI Data")]
public class WoodblockGameData : ScriptableObject
{
    [Header("Story")]
    public TextAsset inkJSONStory;

    [Header("Background")]
    public Color backgroundColor;
    public Texture backgroundImage;

    [Header("Text")]
    public TMP_FontAsset font;
    public Color textBackgroundColor;
    public Color fontColor;
    public float fontSize;
    public float textFadeDuration;
    public int leftMargin;
    public int rightMargin;
    public int lineSpacing;
    public int paragraphSpacing;
    public int textBoxScrollCutoff;

    [Header("Button")]
    public ColorBlock buttonColors;
    public Color buttonFontColor;

    [Header("Progression")]
    public float textSpeed;
    public float storyFadeDuration;
    public float storyFadeDelay;

    [Header("Images")]
    public float imageSpeed;
    public int defaultPortraitHeight;
    public int defaultLandscapeHeight;

    [Header("Menu")]
    public Color menuBackgroundColor;

}
