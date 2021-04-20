using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class GameManager : MonoBehaviour
{
    public GameLoaderSaver loaderSaver;

    public bool shouldAutosave;
    public int autosaveFrequency = 120;

    private void Start()
    {
        StartCoroutine("AutoSaveRoutine");
        loaderSaver.Load();
    }

    public void StartNewGame()
    {
        loaderSaver.DeleteSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Starting new game.");
    }

    public void SaveGame()
    {
        if (loaderSaver.IsLoading())
            return;

        if (loaderSaver.IsSaving())
            return;

        loaderSaver.Save();
    }

    public void QuitGame()
    {
        StartCoroutine("QuitGameRoutine");
    }

    private IEnumerator QuitGameRoutine()
    {
        loaderSaver.Save();

        while (loaderSaver.IsSaving())
            yield return null;

        if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator AutoSaveRoutine()
    {
        for (; ; )
        {
            while (loaderSaver.IsLoading())
            yield return null;

            while (loaderSaver.IsSaving())
            yield return null;

            yield return new WaitForSeconds(autosaveFrequency);
            if (shouldAutosave)
            {
                loaderSaver.Save();
            }
        }
    }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        void OnInspectorGUI()
        {
            var manager = target as GameManager;

            manager.shouldAutosave = GUILayout.Toggle(manager.shouldAutosave, "Should Autosave");

            if (manager.shouldAutosave)
                manager.autosaveFrequency = EditorGUILayout.IntField("Autosave Frequency", manager.autosaveFrequency);

        }
    }
#endif
