using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChunkHolderDisplaySettingsObserver : DisplaySettingsObserver
{
    public VerticalLayoutGroup layoutGroup;
    public override void OnNotify(WoodblockGameData data)
    {
        layoutGroup.padding.left = data.leftMargin;
        layoutGroup.padding.right = data.rightMargin;
    }
}