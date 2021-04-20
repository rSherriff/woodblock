using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DisplaySettingsObserver : MonoBehaviour
{
    public WoodblockGameData data;
    public abstract void OnNotify();
}