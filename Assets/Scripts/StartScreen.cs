using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
}
