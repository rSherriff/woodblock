using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInteractableManager : MonoBehaviour
{
    AssetBundle storyInteractables;

    void Start()
    {
        storyInteractables = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "storyinteractables"));
        if (storyInteractables == null)
        {
            Debug.Log("Failed to load journalPhotographBundle AssetBundle!");
            return;
        }
    }

    public UnityEngine.Object GetStoryinteractable(string name)
    {
        return storyInteractables.LoadAsset(name);
    }
}
