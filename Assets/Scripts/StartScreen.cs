using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public static StartScreen instance;

    [Header("UI Components")]
    [SerializeField]
    TextMeshProUGUI yourName;
    [SerializeField]
    TextMeshProUGUI yourEmail;
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TMP_InputField emailInput;
    [SerializeField]
    Button playButton;

    [Header("Keyboard")]
    [SerializeField]
    Button[] keyboard_part = new Button[12];
    [SerializeField]
    GameObject keyboard;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        nameInput.characterLimit = 5;
    }

    void Update()
    {
        FilledFields();
        ShowKeyboard();
    }

    public void StartQuiz()
    {
        gameObject.SetActive(false);
        Quiz.instance.gameObject.SetActive(true);
    }

    // Metoda proverava da li su polja za unos popunjena
    private void FilledFields()
    {
        if (nameInput.text != "" && nameInput.text.Length > 1)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    // Metoda za prikaz tastature
    private void ShowKeyboard()
    {
        if (nameInput.isFocused)
        {
            KeyboardButtons(true, true);
        }
        else if (emailInput.isFocused)
        {
            KeyboardButtons(true, false);
        }
    }

    //------------------------------------------------------------------------------------------------
    public void KeyboardButtons(bool check, bool name)
    {
        if (name)
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = false;
            }
        }
        else
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = true;
            }
        }
        keyboard.SetActive(check);
    }

}
