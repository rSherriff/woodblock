using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class StoryInteractableBase : MonoBehaviour
{
    //Lifetime of a story interactble. Fade in and pause the story -> Get interacted with. -> Fade out and resume the story.

    [Header("Story Canvas")]
    public InkStoryCanvas storyCanvas;

    [Header("Fading")]
    public CanvasGroup fullScreenFadeCanvas;
    public float fadeDuration = 1.0f;

    [Header("Events")]
    public UnityEvent onEnableEvent;

    private bool isFading = false;
    private bool isStartingUp = false;
    private bool isShuttingDown = false;
    private bool isOpen;

    void Start()
    {
        DisableChildren();
    }

    public void StartUp()
    {
        if(!isStartingUp && !isOpen)
            StartCoroutine("StartupRoutine");
        else
            Debug.Log("Attempted to startup " + name + " when we were already starting up or open!");
    }

    public void Shutdown(string returnChoice = "Back")
    {
        if (!isShuttingDown && isOpen)
            StartCoroutine(ShutdownRotuine(returnChoice));
        else
            Debug.Log("Attempted to shutdown " + name + " when we were already shutting down or not open!");
    }

    private IEnumerator StartupRoutine()
    {
        isStartingUp = true;

        Debug.Log("Starting interactable " + name);
        onEnableEvent.Invoke();
        storyCanvas.DisableProgress();
        StartCoroutine(Fade(1));

        while (isFading)
            yield return null;

        EnableChildren();
        StartCoroutine(Fade(0));

        while (isFading)
            yield return null;
            
        isOpen = true;
        isStartingUp = false;
    }

    private IEnumerator ShutdownRotuine(string returnChoice)
    {
        isShuttingDown = true;

        StartCoroutine(Fade(1));

        while (isFading)
            yield return null;

        DisableChildren();
        storyCanvas.EnableProgress();
        StartCoroutine(Fade(0));

        if(returnChoice.Length > 0)
            storyCanvas.ChooseChoiceString(returnChoice);

        while (isFading)
            yield return null;

        DisableChildren();

        CursorMode cursorMode = CursorMode.Auto;
        Vector2 hotSpot = Vector2.zero;
        Cursor.SetCursor(null, hotSpot, cursorMode);

        isOpen = false;
        isShuttingDown = false;
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        float fadeSpeed = Mathf.Abs(fullScreenFadeCanvas.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(fullScreenFadeCanvas.alpha, finalAlpha))
        {
            fullScreenFadeCanvas.alpha = Mathf.MoveTowards(fullScreenFadeCanvas.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
    }

    void DisableChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void EnableChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public bool IsInteractableOpen()
    {
        return isOpen;
    }

    public bool IsShuttingDown()
    {
        return isShuttingDown;
    }

    public bool IsStartingUp()
    {
        return isStartingUp;
    }
}