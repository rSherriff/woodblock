using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonMenuDisplaySettingsObserver : DisplaySettingsObserver
{
    public Button button;
    public TextMeshProUGUI text;
    public override void OnNotify(WoodblockGameData data)
    {
        text.font = data.font;
        text.color = data.buttonFontColor;
    }
}