using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameLoaderSaver : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    private bool isLoading;
    private bool isSaving;

    private GameStateBase state;

    private void Start()
    {
        state = new GameStateBase();
    }

    public void Save()
    {
        StartCoroutine(SaveRoutine());
    }

    private IEnumerator SaveRoutine()
    {
        isSaving = true;

        Debug.Log("Saving game to: " + Application.persistentDataPath);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +  "/saveData.med");

        InkStoryCanvas story = FindObjectOfType<InkStoryCanvas>();

        state.version = Application.version;
        state.storyState = story.GetStory().GetStoryJSON();
        state.textChunks = story.GetAllTextChunks();
        state.ambientMusic = FindObjectOfType<AmbientMusic>().GetCurrentTrackName();

        state.openInteractable = "";
        for (int i = 0; i < story.interactablesHolder.childCount; i++)
        {
            if (story.interactablesHolder.GetChild(i).GetComponent<StoryInteractableBase>().IsInteractableOpen())
            {
                Debug.Log(story.interactablesHolder.GetChild(i).name + " is open on save, we will open it again on load");
                state.openInteractable = story.interactablesHolder.GetChild(i).name;
            }
        }

        bf.Serialize(file, state);
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SyncFiles();
        }

        Debug.Log("Saved game.");

        isSaving = false;

        yield break;
    }

    public void Load()
    {
        StartCoroutine(LoadRoutine());
    }

    public IEnumerator LoadRoutine()
    {
        Debug.Log("Loading game from: " + Application.persistentDataPath);

        isLoading = true;
        if (File.Exists(Application.persistentDataPath + "/saveData.med"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.med", FileMode.Open);
            state = (GameStateBase)bf.Deserialize(file);

            //Wait until the story has been set up
            InkStoryCanvas story = null;
            while (story == null)
            {
                story = FindObjectOfType<InkStoryCanvas>();
                yield return null;
            }

            //If there is no state there is either no data or its failed
            if (state == null)
            {
                isLoading = false;
            }

            if (state.version != Application.version)
            {
                isLoading = false;
            }

            FindObjectOfType<AmbientMusic>().StartNewTrack(state.ambientMusic);

            if (state.storyState != null)
                story.GetStory().LoadFromJSON(state.storyState);

            if (state.textChunks.Count > 0)
            {
                for (int i = 0; i < state.textChunks.Count; i++)
                {
                    story.AddTextChunk(state.textChunks[i]);
                }
            }

            if (state.openInteractable.Length > 0)
            {
                story.StartInteractable(state.openInteractable);
            }

            file.Close();
        }

        isLoading = false;
    }

    public void DeleteSave()
    {
        string filePath = Application.persistentDataPath + "/saveData.med";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public bool IsLoading()
    {
        return isLoading;
    }

    public bool IsSaving()
    {
        return isSaving;
    }
}
