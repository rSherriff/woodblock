using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StoryInteractableBase))]
class StoryInteractableBaseEditor : Editor
{
    StoryInteractableBase script;
    GameObject scriptObject;

    void OnEnable()
    {
        script = (StoryInteractableBase)target;
        scriptObject = script.gameObject;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Enable Children"))
        {
            foreach (Transform child in scriptObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        if (GUILayout.Button("Disable Children"))
        {
            foreach (Transform child in scriptObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
