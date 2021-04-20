using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class InkStoryCanvas : MonoBehaviour
{
    public WoodblockGameData data;
    public InkStoryAdaptor story;

    [Header("UI Prefabs")]
    public Button verticalButtonPrefab;
    public Button gridButtonPrefab;
    public TextChunk textPrefab;
    public ImageChunk imagePrefab;

    [Header("UI Layout")]
    public GameObject textRect;
    public GameObject buttonRectVertical;
    public GameObject buttonRectGrid;
    public ScrollRect scrollRect;
    private int maxVerticalChoices = 5;
    

    [Header("UI Canvas Groups")]
    public CanvasGroup textCanvasGroup;
    public CanvasGroup buttonCanvasGroup;

    [Header("Clap")]
    public GameObject clapPrefab;

    private bool storyStarted = false;
    private bool readingTags = false;

    [Header("Interactables")]
    public Transform interactablesHolder;

    [Header("Display Observers")]
    public List<DisplaySettingsObserver> displayObservers;

    void Start()
    {
        if (story.SetupOk())
        {
            ClearUI();

            // Start the refresh cycle
            StartCoroutine(FirstRefresh());
        }

        foreach (DisplaySettingsObserver ob in displayObservers)
        {
            ob.OnNotify();
        }
    }

    void Update()
    {

    }

    private IEnumerator FirstRefresh()
    {
        StartCoroutine(Fade(textCanvasGroup, data.storyFadeDuration, 1));
        yield return new WaitForSeconds(data.storyFadeDuration);

        StartCoroutine(Refresh());
    }

    // Refresh the UI elements
    //  - Clear any current elements
    //  - Read any tags in chunks
    //  - Show any text chunks
    //  - Iterate through any choices and create listeners on them
    private IEnumerator Refresh()
    {
        ClearButtons();

        //Get the next story block
        List<StoryLine> nextTextBoxLines = story.GetNextStoryBlock();

        //Read tags and wait while they are setting up
        StartCoroutine(ReadTags(nextTextBoxLines));
        while (readingTags)
            yield return null;

        //Should the next lines get a text chunk?
        if (ShouldAddTextChunk(nextTextBoxLines))
        {
            //Construct the text box out of the story lines
            TextChunk chunk = AddTextChunk(nextTextBoxLines);

            //Wait while the chuck is setting up
            while (chunk.IsSettingUp())
                yield return null;

            yield return new WaitForSeconds(0.1f);
        }

        AddChoiceButtons();

        yield break;
    }

    //Add the next set of choice buttons under the text
    private void AddChoiceButtons()
    {
        //Set up choices
        if (story.GetCurrentChoices().Count > 0)
        {
            int choiceNumber = 1;
            int numChoices = story.GetCurrentChoices().Count;
            foreach (Choice choice in story.GetCurrentChoices())
            {
                Button choiceButton = null;

                if (numChoices <= maxVerticalChoices)
                {
                    choiceButton = Instantiate(verticalButtonPrefab) as Button;
                    choiceButton.transform.SetParent(buttonRectVertical.transform, false);
                }
                else
                {
                    choiceButton = Instantiate(gridButtonPrefab) as Button;
                    choiceButton.transform.SetParent(buttonRectGrid.transform, false);
                }

                // Gets the text from the button prefab
                TMPro.TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                choiceText.text = choice.text;

                // Set listener
                choiceButton.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });

                choiceNumber++;

                CanvasGroup cg = choiceButton.GetComponent<CanvasGroup>();
                cg.alpha = 0;
                StartCoroutine(Fade(cg, data.storyFadeDuration, 1, data.storyFadeDelay * choiceNumber));
            }
        }
        else
        {
            //If we're not running in a browser, create a 'Quit' button when the story ends.
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Button choiceButton = Instantiate(verticalButtonPrefab) as Button;
                choiceButton.transform.SetParent(buttonRectVertical.transform, false);

                // Gets the text from the button prefab
                TMPro.TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                choiceText.text = "Quit";

                // Set listener
                choiceButton.onClick.AddListener(EndStory);

                //Fade In
                CanvasGroup cg = choiceButton.GetComponent<CanvasGroup>();
                cg.alpha = 0;
                StartCoroutine(Fade(cg, data.storyFadeDuration, 1, data.storyFadeDuration));
            }

        }
    }

    private IEnumerator ReadTags(List<StoryLine> nextTextBoxLines)
    {
        //Scan the next story block to see if it has any tags that need to be read at this level
        foreach (StoryLine line in nextTextBoxLines)
        {
            foreach (string t in line.tags)
            {
                if (t.StartsWith("IMAGE", System.StringComparison.CurrentCulture))
                {
                    ImageChunk image = Instantiate(imagePrefab) as ImageChunk;
                    image.transform.SetParent(textRect.transform, false);
                    string[] imgArgs = t.Split();
                    if (imgArgs.Length > 1)
                    {
                        int desiredWidth = 0, desiredHeight = 0;

                        string img = imgArgs[1];
                        if (imgArgs.Length >= 4) 
                        {
                            desiredWidth = Int32.Parse(imgArgs[2]);
                            desiredHeight = Int32.Parse(imgArgs[3]);
                        }
                        image.SetupImage(scrollRect, t.Split()[1], desiredWidth, desiredHeight);
                    }

                    while (image.IsSettingUp())
                        yield return null;
                }
                else if (t.StartsWith("CLAP", System.StringComparison.CurrentCulture))
                {
                    string clapString = t.Split()[1];
                    GameObject clap = Instantiate(clapPrefab);
                    clap.GetComponent<Clap>().Setup(clapString);
                }
                else if (t.StartsWith("AMBIENT", System.StringComparison.CurrentCulture))
                {
                    FindObjectOfType<AmbientMusic>().StartNewTrack(t.Split()[1]);
                }
                else if (t.StartsWith("INTERACTABLE", System.StringComparison.CurrentCulture))
                {
                    string interactableName = t.Split()[1];
                    StartInteractable(interactableName);
                }
                else if (t.StartsWith("CLEAR", System.StringComparison.CurrentCulture))
                {
                    ClearText();
                }
            }
        }
    }

    //Add Text chunk
    public TextChunk AddTextChunk(List<StoryLine> nextTextBoxLines)
    {
        TextChunk chunk = Instantiate(textPrefab) as TextChunk;
        chunk.transform.SetParent(textRect.transform, false);
        chunk.SetupText(scrollRect, nextTextBoxLines, !storyStarted);
        storyStarted = true;

        return chunk;
    }

    //Add Text chunk
    public void AddTextChunk(string nextTextBoxLines)
    {
        StoryLine line = new StoryLine();
        line.text = nextTextBoxLines;

        TextChunk chunk = Instantiate(textPrefab) as TextChunk;
        chunk.transform.SetParent(textRect.transform, false);
        chunk.SetupText(scrollRect, line, true);
    }

    public bool ShouldAddTextChunk(List<StoryLine> nextTextBoxLines)
    {
        if (nextTextBoxLines.Count <= 0)
            return false;

        foreach(StoryLine line in nextTextBoxLines)
        { 
            string stripped = Regex.Replace(line.text, @"\t|\n|\r", "");

            if (nextTextBoxLines.Count > 0 && stripped.Length > 0)
                return true;
        }

        return false;
    }

    //End the current Story
    public void EndStory()
    {
        //StartCoroutine(FadeAndEnd());
        Application.Quit();
    }

    //Fade out before we allow ourselves to end. old, should probably be done with full screen fade
    IEnumerator FadeAndEnd()
    {
        StartCoroutine(Fade(textCanvasGroup, data.storyFadeDuration, 0));
        StartCoroutine(Fade(buttonCanvasGroup, data.storyFadeDuration, 0));
        yield return new WaitForSeconds(data.storyFadeDuration);
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        StartCoroutine(Refresh());
    }

    // Clear out all of the UI
    void ClearUI()
    {
        ClearText();
        ClearButtons();
    }

    //Clear out text, calling Destory() in reverse
    void ClearText()
    {
        int childCount = textRect.transform.childCount;
        for (int i = childCount - 1; i >= 1; --i)//Delete all chunks but one (the initial spacer chunk)
        {
            Destroy(textRect.transform.GetChild(i).gameObject);
        }

    }

    //Clear out buttons, calling Destory() in reverse
    void ClearButtons()
    {
        int childCount = buttonRectVertical.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            Destroy(buttonRectVertical.transform.GetChild(i).gameObject);
        }

        childCount = buttonRectGrid.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            Destroy(buttonRectGrid.transform.GetChild(i).gameObject);
        }
    }

    //Fade util function that I have written a million times and never found a nice place for.
    private IEnumerator Fade(CanvasGroup canvasGroup, float duration, float finalAlpha, float delay = 0)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        if (!canvasGroup)
            yield break;

        canvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / duration;

        while (canvasGroup && !Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }
    }

    //Is a particular choice present at this story segment
    public bool IsChoicePresent(string c)
    {
        foreach (Choice choice in story.GetCurrentChoices())
        {
            if (c == choice.text)
                return true;
        }

        return false;
    }

    //Make a choice without using the buttons. This is used for navigation and returning from interactables, but I'm not forcing it to be them because I'm nice.
    //Anywhere else will need justification.
    public void ChooseChoiceString(string choice)
    {
        int index = 0;
        foreach (Choice c in story.GetCurrentChoices())
        {
            if (c.text == choice)
            {
                story.ChooseChoiceIndex(index);
                StopAllCoroutines();
                StartCoroutine(Refresh());
                return;
            }
            index++;
        }
        Debug.LogError("Called (" + choice + ") as a choice but it doesn't exist!");
    }

    //Stop the player using the buttons or compass to progress
    public void DisableProgress()
    {
        buttonRectVertical.gameObject.SetActive(false);
    }

    //Enable buttons and compass for progression.
    public void EnableProgress()
    {
        buttonRectVertical.gameObject.SetActive(true);
    }

    //Get current knot, used for saving
    public string GetCurrentKnot()
    {
        return story.GetCurrentPathString();
    }

    //Get story object
    public InkStoryAdaptor GetStory()
    {
        return story;
    }

    //Set story object
    public void SetStory(InkStoryAdaptor newStory)
    {
        story = newStory;
    }

    public List<string> GetAllTextChunks()
    {
        List<string> strings = new List<string>();

        for(int i = 0; i < textRect.transform.childCount; i++)
        {
            GameObject c = textRect.transform.GetChild(i).gameObject;
            if(c.GetComponent<TextChunk>())
            {
                strings.Add(c.GetComponent<TextChunk>().GetText());
            }
        }

        return strings;
    }

    public void StartInteractable(string interactableName)
    {
        bool interactableFound = false;
        for (int i = 0; i < interactablesHolder.childCount; i++)
        {
            StoryInteractableBase c = interactablesHolder.GetChild(i).GetComponent<StoryInteractableBase>();
            if (c.name == interactableName)
            {
                c.StartUp();
                interactableFound = true;
                break;
            }
        }

        if (!interactableFound)
            Debug.LogError(interactableName + " not found in InkExample.interactables");
    }
}