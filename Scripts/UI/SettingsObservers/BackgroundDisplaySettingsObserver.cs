using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackgroundDisplaySettingsObserver : DisplaySettingsObserver
{
    public override void OnNotify()
    {
        GetComponent<RawImage>().texture = data.backgroundImage;
        GetComponent<RawImage>().color = data.backgroundColor;
    }
}
