using System;
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
        if (!UsernameCheck() || !EmailCheck() || !PasswordCheck() || !CheckConfirmPassword())
            return;
        else
        {
            Debug.Log("Sve je u redu");
        }
    }

    private bool UsernameCheck()
    {
        Regex usernameRegex = new Regex("^[a-zA-Z0-9]+$");
        UIManager.Instance.message.text = "";

        if (usernameRegex.IsMatch(usernameInput.text))
        {
            Debug.Log("Validan username");
            return true;
        }
        else
        {
            UIManager.Instance.message.text = "The username is not valid!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool EmailCheck()
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        UIManager.Instance.message.text = "";
        if (emailRegex.IsMatch(emailInput.text))
        {
            Debug.Log("Validan meil");
            return true;
        }
        else
        {
            UIManager.Instance.message.text = "The email is not valid!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool PasswordCheck()
    {
        Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        UIManager.Instance.message.text = "";
        if (passwordRegex.IsMatch(passwordInput.text))
        {
            Debug.Log("Validan pasword");
            return true;
        }
        else
        {
            // UIManager.Instance.message.text = "The password must be at least 6 characters long and consist of uppercase and lowercase letters, symbols, and numbers";
            UIManager.Instance.message.text = "The password is not valid!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool CheckConfirmPassword()
    {
        if (passwordInput.text.Equals(confirmPasswordInput.text))
        {
            Debug.Log("Potvrđen password");
            return true;
        }
        else
        {
            UIManager.Instance.message.text = "The password is not validddd!";
            UIManager.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }
}
