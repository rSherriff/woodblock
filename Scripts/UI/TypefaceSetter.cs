using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypefaceSetter : MonoBehaviour
{
    public WoodblockGameData data;

    public void Start()
    {
        GetComponent<TextMeshProUGUI>().font = data.font;
    }
}
