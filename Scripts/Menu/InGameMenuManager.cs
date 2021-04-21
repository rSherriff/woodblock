using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject panel;

    public Button newGameButton;
    public Button optionsButton;
    public Button quitButton;
    
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider ambientSlider;
    public Slider effectsSlider;

    public GameObject optionsPanel;
    public GameObject closeButton;

    public FadeCanvasGroupInOut saveText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        float parameter;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            mixer.GetFloat("MasterVolume", out parameter);
            masterSlider.value = parameter;
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        }
        else
        {
            mixer.GetFloat("EffectsVolume", out parameter);
            effectsSlider.value = parameter;
        }

        if (PlayerPrefs.HasKey("AmbientVolume"))
        {
            ambientSlider.value = PlayerPrefs.GetFloat("AmbientVolume");
        }
        else
        {
            mixer.GetFloat("AmbientVolume", out parameter);
            ambientSlider.value = parameter;
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            quitButton.gameObject.SetActive(false);
    }

    void Update()
    {
    }

    public void OnMainIconClick()
    {
        optionsPanel.SetActive(false);
        panel.SetActive(true);
        closeButton.SetActive(true);
    }

    public void OnMainPanelClick()
    {
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OnNewGameClick()
    {
        gameManager.StartNewGame();
        panel.SetActive(false);
    }

    public void OnSaveGameClick()
    {
        gameManager.SaveGame();
        saveText.StartFade();
        panel.SetActive(false);
    }

    public void OnOptionsClick()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void OnQuitClick()
    {
        gameManager.QuitGame();
        panel.SetActive(false);
    }

    public void SetMasterLevel()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetAmbientLevel()
    {
        mixer.SetFloat("AmbientVolume", ambientSlider.value);
        PlayerPrefs.SetFloat("AmbientVolume", ambientSlider.value);
    }

    public void SetEffectsLevel()
    {
        mixer.SetFloat("EffectsVolume", effectsSlider.value);
        PlayerPrefs.SetFloat("EffectsVolume", effectsSlider.value);
    }

    public void Close()
    {
        panel.SetActive(false);
        closeButton.SetActive(false);
        optionsPanel.SetActive(false);
    }
}
