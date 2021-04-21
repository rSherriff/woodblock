using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChunkDisplaySettingsObserver : DisplaySettingsObserver
{
    public TextMeshProUGUI text;
    public Image backgroundImage;

    public override void OnNotify(WoodblockGameData data)
    {
        text.font = data.font;
        text.color = data.fontColor;
        text.fontSize = data.fontSize;
        text.lineSpacing = data.lineSpacing;
        text.paragraphSpacing = data.paragraphSpacing;
        backgroundImage.color = data.textBackgroundColor;
    }
}