using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegistrationScreen : MonoBehaviour
{
    private static RegistrationScreen instance;
    public static RegistrationScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("RegistrationScreen is null");
            return instance;
        }
    }

    [Header("UI Components")]
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    [SerializeField]
    Button registerButton;

    [Header("Keyboard")]
    public GameObject virtualKeyboard;



    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        FieldFilled();
    }

    void FieldFilled()
    {
        if ((usernameInput.text != "" && usernameInput.text.Length > 1) && (emailInput.text != "" && emailInput.text.Length > 1) && (passwordInput.text != "" && passwordInput.text.Length > 1) && (confirmPasswordInput.text != "" && confirmPasswordInput.text.Length > 1))
        {
            registerButton.interactable = true;
        }
        else
        {
            registerButton.interactable = false;
        }
    }

    public void Registration()
    {
        UsernameCheck();
        EmailCheck();
    }

    private void UsernameCheck()
    {
        Regex usernameRegex = new Regex("^[a-zA-Z0-9]+$");
        UIManager.Instance.message.text = "";

        if (usernameRegex.IsMatch(emailInput.text))
        {
            Debug.Log("Validan meil");
        }
        else
        {
            UIManager.Instance.message.text = "The email is not valid!";
            UIManager.Instance.messagePanel.SetActive(true);
        }
    }

    public void EmailCheck()
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        UIManager.Instance.message.text = "";
        if (emailRegex.IsMatch(emailInput.text) && (emailInput.text != ""))
        {
            Debug.Log("Validan meil");
        }
        else
        {
            UIManager.Instance.message.text = "The email is not valid!";
        }
        UIManager.Instance.messagePanel.SetActive(true);
    }


}
