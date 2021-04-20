using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class ChoiceButton : MonoBehaviour
{
    public WoodblockGameData data;

    private CanvasGroup faderCanvasGroup;

    private Button button;
    public TextMeshProUGUI buttonText;

    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Update()
    {
        if (Application.isEditor)
        {
            refreshData();
        }
    }

    public void refreshData()
    {
        button.colors = data.buttonColors;

        buttonText.font = data.font;
        buttonText.color = data.buttonFontColor;
        
    }

}
