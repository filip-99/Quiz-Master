using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using IneryLibrary;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary.Core.Api.v1;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

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

    private UserData jsonObj;
    public QuestionSO questionDataListContainer;

    [Header("UI Components")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [SerializeField]
    Button playButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Debug.Log(scriptableObject.userData.username.ToString());
        usernameInput.text = BlockchainData.Instance.GetQuestions().Count.ToString();
    }

    void Update()
    {
        FilledFields();
    }



    public void StartQuiz()
    {
        if (!UsernameCheck() || !PasswordCheck())
        {
            return;
        }
        else
        {
            MediatorScript.Instance.GetRow(usernameInput.text);
            var jsonObj = MediatorScript.Instance.jsonObj;
            Debug.Log(jsonObj.username);
            if ((usernameInput.text.Equals(jsonObj.username)) && (RegistrationScreen.HashPassword(passwordInput.text).Equals(jsonObj.password)))
            {
                UserData user = new UserData();
                user.username = jsonObj.username;
                user.password = jsonObj.password;
                user.email = jsonObj.email;
                user.user_id = jsonObj.user_id;
                user.max_score = jsonObj.max_score;

                // scriptableObject.SaveUserData(user);

                MediatorScript.Instance.StartGame();
            }
            else
            {
                Debug.Log("Korisnik ne postoji");

                UIManagerScene2.Instance.message.text = "The user does not exist, please try again.";
                UIManagerScene2.Instance.ShowPanel(gameObject.transform);

                return;
            }

        }
    }

    // Metoda proverava da li su polja za unos popunjena
    private void FilledFields()
    {
        if ((usernameInput.text != "" && usernameInput.text.Length > 1) && (passwordInput.text != "" && passwordInput.text.Length > 1))
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    private bool UsernameCheck()
    {
        Regex usernameRegex = new Regex("^[a-z1-5.]{1,12}$");
        UIManagerScene2.Instance.message.text = "";

        if (usernameRegex.IsMatch(usernameInput.text))
        {
            Debug.Log("Validan username");
            return true;
        }
        else
        {
            UIManagerScene2.Instance.message.text = "The username is not valid!";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }

    public bool PasswordCheck()
    {
        Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        UIManagerScene2.Instance.message.text = "";
        if (passwordRegex.IsMatch(passwordInput.text))
        {
            Debug.Log("Validan pasword");
            return true;
        }
        else
        {
            // UIManager.Instance.message.text = "The password must be at least 6 characters long and consist of uppercase and lowercase letters, symbols, and numbers";
            UIManagerScene2.Instance.message.text = "The password is not valid!";
            UIManagerScene2.Instance.ShowPanel(gameObject.transform);
            return false;
        }
    }
}
