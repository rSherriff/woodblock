using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplaySettingsObserver : DisplaySettingsObserver
{
    public TextMeshProUGUI text;
    public override void OnNotify(WoodblockGameData data)
    {
        text.font = data.font;
        text.color = data.buttonFontColor;
    }
}