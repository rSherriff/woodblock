using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateBase
{
    public string version;
    public string storyState;
    public List<string> textChunks;
    public string openInteractable;
    public string ambientMusic;
}
