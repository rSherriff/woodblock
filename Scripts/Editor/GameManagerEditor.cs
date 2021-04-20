using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var manager = target as GameManager;

        manager.shouldAutosave = GUILayout.Toggle(manager.shouldAutosave, "Should Autosave");

        if (manager.shouldAutosave)
            manager.autosaveFrequency = EditorGUILayout.IntField("Autosave Frequency", manager.autosaveFrequency);

    }
}
