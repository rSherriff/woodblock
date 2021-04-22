using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonMenuDisplaySettingsObserver : DisplaySettingsObserver
{
    public Button button;
    public TextMeshProUGUI text;
    public override void OnNotify(WoodblockGameData data)
    {
        button.colors = data.menuButtonColors;

        text.font = data.font;
        text.color = data.menuButtonFontColor;
    }
}