using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    static StartScreen instance;
    public static StartScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("StartScreen is null");
            return instance;
        }
    }

    [Header("UI Components")]
    public TextMeshProUGUI yourName;
    public TextMeshProUGUI yourEmail;
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;

    [SerializeField]
    Button playButton;

    [Header("MessagePanel")]
    [SerializeField]
    private GameObject MessagePanel;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TextMeshProUGUI message;

    [Header("Keyboard")]
    public GameObject virtualKeyboard;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        nameInput.characterLimit = 8;
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
        if (nameInput.text != "" && nameInput.text.Length > 1)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }

        Keyboard.Instance.ShowKeyboard();
    }

    // Metoda za pušovanje meila
    public void Subscribe()
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        message.text = "";
        if (emailRegex.IsMatch(emailInput.text))
        {
            HighscoreHandler.Instance.Subscribe(emailInput.text, emailInput, message);
        }
        else
        {
            message.text = "The email is not valid!";
        }
        MessagePanel.SetActive(true);
    }

    // Zatvaranje panela
    public void ClickCancel()
    {
        MessagePanel.SetActive(false);
    }
}
