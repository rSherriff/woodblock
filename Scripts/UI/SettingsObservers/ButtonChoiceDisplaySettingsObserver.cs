using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonChoiceDisplaySettingsObserver : DisplaySettingsObserver
{
    public Button button;
    public TextMeshProUGUI text;
    public override void OnNotify(WoodblockGameData data)
    {
        button.colors = data.buttonColors;

        text.font = data.font;
        text.color = data.buttonFontColor;
    }
}