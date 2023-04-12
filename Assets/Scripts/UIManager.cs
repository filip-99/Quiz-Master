using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("UIManager is null");
            return instance;
        }
    }

    [Header("MessagePanel")]
    public GameObject messagePanel;
    public TextMeshProUGUI message;
    public Button cancelButton;

    [Header("Keyboard")]
    public GameObject virtualKeyboard;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartScreen.Instance.gameObject.SetActive(true);

        Quiz.Instance.gameObject.SetActive(false);

        EndScreen.Instance.gameObject.SetActive(false);

        RegistrationScreen.Instance.gameObject.SetActive(false);

        LoginScreen.Instance.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    // Povratak na početni ekran
    public void Back()
    {
        RegistrationScreen.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(false);

        StartScreen.Instance.gameObject.SetActive(true);
    }

    // Isključivanje panela za obaveštenje
    public void ClickCancel()
    {
        messagePanel.SetActive(false);
    }

    // Metoda za prikaz tastature
    public void ShowKeyboard()
    {
        if ((RegistrationScreen.Instance.usernameInput.isFocused))
        {
            virtualKeyboard.SetActive(true);
        }
        else if ((RegistrationScreen.Instance.emailInput.isFocused) || (RegistrationScreen.Instance.passwordInput.isFocused) || (RegistrationScreen.Instance.confirmPasswordInput.isFocused))
        {
            virtualKeyboard.SetActive(true);
        }
    }
}
