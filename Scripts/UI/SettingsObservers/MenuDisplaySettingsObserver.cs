using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuDisplaySettingsObserver : DisplaySettingsObserver
{
    public Image image;
    public override void OnNotify(WoodblockGameData data)
    {
        image.color = data.menuBackgroundColor;
    }
}