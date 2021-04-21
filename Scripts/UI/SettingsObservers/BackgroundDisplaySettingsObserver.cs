using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackgroundDisplaySettingsObserver : DisplaySettingsObserver
{
    public RawImage image;
    public override void OnNotify(WoodblockGameData data)
    {
        image.texture = data.backgroundImage;
        image.color = data.backgroundColor;
    }
}
