using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class TextChunkHolder : MonoBehaviour
{
    public WoodblockGameData data;
    private VerticalLayoutGroup layoutGroup;

    void Start()
    {
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        refreshSettings();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            refreshSettings();
        }
    }

    void refreshSettings()
    {
        layoutGroup.padding.left = data.leftMargin;
        layoutGroup.padding.right = data.rightMargin;
    }

}
