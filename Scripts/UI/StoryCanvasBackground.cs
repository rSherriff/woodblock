using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class StoryCanvasBackground : MonoBehaviour
{
    public WoodblockGameData data;
    private RawImage image;

    void Start()
    {
        image = GetComponent<RawImage>();
        RefreshSettings();
    }

    void Update()
    {
        if (Application.isEditor)
        {
            RefreshSettings();
        }
    }

    void RefreshSettings()
    {
        image.texture = data.backgroundImage;
        image.color = data.backgroundColor;
    }
}
