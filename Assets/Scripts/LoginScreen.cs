using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    static LoginScreen instance;
    public static LoginScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("StartScreen is null");
            return instance;
        }
    }

    [Header("UI Components")]
    public TMP_InputField usernameInput;
    [SerializeField]
    Button playButton;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        usernameInput.characterLimit = 8;
    }

    void Update()
    {
        FilledFields();
    }

    public void StartQuiz()
    {
        gameObject.SetActive(false);
        Quiz.Instance.gameObject.SetActive(true);
    }

    // Metoda proverava da li su polja za unos popunjena
    private void FilledFields()
    {
        if (usernameInput.text != "" && usernameInput.text.Length > 1)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
        UIManager.Instance.ShowKeyboard();
    }
}
