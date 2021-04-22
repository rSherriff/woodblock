using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "WoodblockData", menuName = "Woodblock/Woodblock Data")]
public class WoodblockGameData : ScriptableObject
{
    private bool refreshGame;

    [Header("Story")]
    public TextAsset inkJSONStory;

    [Header("Background")]
    public Color backgroundColor = Color.white;
    public Texture backgroundImage;

    [Header("Text")]
    public TMP_FontAsset font;
    public Color textBackgroundColor = new Color(0,0,0,0);
    public Color fontColor = Color.black;
    public float fontSize = 15;
    public int leftMargin =150;
    public int rightMargin = 150;
    public int lineSpacing = 30;
    public int paragraphSpacing = 60;
    public int textBoxScrollCutoff = 250;

    [Header("Button")]
    public ColorBlock buttonColors  = ColorBlock.defaultColorBlock;
    public Color buttonFontColor = Color.black;

    [Header("Progression")]
    public float textSpeed = 200;
    public float storyFadeDuration = 0.2f;
    public float storyFadeDelay = 0.2f;

    [Header("Images")]
    public float imageSpeed = 200;
    public int defaultPortraitHeight = 200;
    public int defaultLandscapeHeight = 200;

    [Header("Menu")]
    public Color menuBackgroundColor = new Color(0, 0, 0, 128);
    [Header("Menu Buttons")]
    public ColorBlock menuButtonColors = ColorBlock.defaultColorBlock;
    public Color menuButtonFontColor = Color.black;

    public void SetRefreshGame(bool value)
    {
        refreshGame = value;
    }

    public bool ShouldRefreshGame()
    {
        return refreshGame;
    }
}
