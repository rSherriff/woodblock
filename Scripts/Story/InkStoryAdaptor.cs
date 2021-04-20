using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

#if (UNITY_EDITOR)
using UnityEditor;
using Ink.UnityIntegration;
#endif

public struct StoryLine
{
    public string text;
    public List<string> tags;
}

[System.Serializable]
public class InkStoryAdaptor : MonoBehaviour
{
    public WoodblockGameData data;
    public static event Action<Story> OnCreateStory;

#if (UNITY_EDITOR)
    public bool attachStoryOnPlay;
#endif

    protected Story story;
    private string lastKnot;

    void Awake()
    {
        if (data.inkJSONStory != null)
        {
            Debug.Log("Loading Story " + data.inkJSONStory.name);

            story = new Story(data.inkJSONStory.text);
            lastKnot = "";
#if (UNITY_EDITOR)
            if (OnCreateStory != null && attachStoryOnPlay) OnCreateStory(story);
#endif
        }
        else
        {
            Debug.LogError("Can't find an Ink story! This is going to be a short game!");
        }
    }

    public List<Choice> GetCurrentChoices()
    {
        return story.currentChoices;
    }

    public void ChooseChoiceIndex(int index)
    {
        story.ChooseChoiceIndex(index);
    }

    public void ChoosePathString(string path)
    {
        story.ChoosePathString(path);
    }

    public string GetCurrentPathString()
    {
        return lastKnot;
    }

    public string GetVariable(string variableName)
    {
        return story.variablesState[variableName].ToString();
    }

    public string GetStoryJSON()
    {
        return story.state.ToJson();
    }

    public void LoadFromJSON(string state)
    {
        story.state.LoadJson(state);
    }

    // Load and potentially return the next story block
    public List<StoryLine> GetNextStoryBlock()
    {
        List<StoryLine> lines = new List<StoryLine>();

        while (story.canContinue)
        {
            StoryLine line = new StoryLine();

            if (story.state.currentPathString != "Null")
                lastKnot = story.state.currentPathString.Split('.')[0];

            line.text = story.Continue();

            line.tags = story.currentTags;
            lines.Add(line);
        }

        return lines;
    }

    public bool SetupOk()
    {
        return story != null;
    }


#if (UNITY_EDITOR)
    [CustomEditor(typeof(InkStoryAdaptor))]
    [InitializeOnLoad]
    public class InkStoryAdaptorEditor : Editor
    {
        static InkStoryAdaptorEditor()
        {
            InkStoryAdaptor.OnCreateStory += OnCreateStory;
        }

        static void OnCreateStory(Story story )
        {
            InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
            if (window != null) InkPlayerWindow.Attach(story);
        }
        public override void OnInspectorGUI()
        {
            Repaint();
            base.OnInspectorGUI();
            var realTarget = target as InkStoryAdaptor;
            var story = realTarget.story;
            InkPlayerWindow.DrawStoryPropertyField(story, new GUIContent("Story"));
        }
    }
#endif
}
