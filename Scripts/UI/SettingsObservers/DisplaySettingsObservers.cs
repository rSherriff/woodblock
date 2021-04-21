using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DisplaySettingsObserver : MonoBehaviour
{
    public abstract void OnNotify(WoodblockGameData data);
}