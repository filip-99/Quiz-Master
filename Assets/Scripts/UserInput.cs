using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    // Registration
    string wordUsername = null;
    string wordEmail = null;
    string wordPassword = null;
    string wordConfirmPassword = null;
    int wordUsernameIndex = 0;
    int wordEmaildIndex = 0;
    int wordPasswordIndex = 0;
    int wordConfirmPasswordIndex = 0;
    string alpha;
    int focusElement;

    // Login
    string wordUsernameLogin = null;
    string wordPasswordLogin = null;
    int wordUsernameIndexLogin = 0;
    int wordPasswordIndexLogin = 0;

    void Update()
    {
        if (RegistrationScreen.Instance.usernameInput.isFocused)
        {
            focusElement = 1;
        }
        else if (RegistrationScreen.Instance.emailInput.isFocused)
        {
            focusElement = 2;
        }
        else if (RegistrationScreen.Instance.passwordInput.isFocused)
        {
            focusElement = 3;
        }
        else if (RegistrationScreen.Instance.confirmPasswordInput.isFocused)
        {
            focusElement = 4;
        }
        // ------------------------------------------------------------------------
        else if (LoginScreen.Instance.usernameInput.isFocused)
        {
            focusElement = 5;
        }
        else if (LoginScreen.Instance.passwordInput.isFocused)
        {
            focusElement = 6;
        }
    }

    public void AlphabetFunction(string alphabet)
    {

        if (alphabet == "bs")
        {
            switch (focusElement)
            {
                case 1:
                    wordUsernameIndex--;
                    if (wordUsername.Length > 0)
                        wordUsername = wordUsername.Remove(wordUsername.Length - 1, 1);
                    RegistrationScreen.Instance.usernameInput.text = wordUsername.ToString();
                    break;

                case 2:
                    wordEmaildIndex--;
                    if (wordEmail.Length > 0)
                        wordEmail = wordEmail.Remove(wordEmail.Length - 1, 1);
                    RegistrationScreen.Instance.emailInput.text = wordEmail.ToString();
                    break;

                case 3:
                    wordPasswordIndex--;
                    if (wordPassword.Length > 0)
                        wordPassword = wordPassword.Remove(wordPassword.Length - 1, 1);
                    RegistrationScreen.Instance.passwordInput.text = wordPassword.ToString();
                    break;


                case 4:
                    wordConfirmPasswordIndex--;
                    if (wordConfirmPassword.Length > 0)
                        wordConfirmPassword = wordConfirmPassword.Remove(wordConfirmPassword.Length - 1, 1);
                    RegistrationScreen.Instance.confirmPasswordInput.text = wordConfirmPassword.ToString();
                    break;
                case 5:
                    wordUsernameIndexLogin--;
                    if (wordUsernameLogin.Length > 0)
                        wordUsernameLogin = wordUsernameLogin.Remove(wordUsernameLogin.Length - 1, 1);
                    LoginScreen.Instance.usernameInput.text = wordUsernameLogin.ToString();
                    break;
                case 6:
                    wordPasswordIndexLogin--;
                    if (wordPasswordLogin.Length > 0)
                        wordPasswordLogin = wordPasswordLogin.Remove(wordPasswordLogin.Length - 1, 1);
                    LoginScreen.Instance.passwordInput.text = wordPasswordLogin.ToString();
                    break;
                default:
                    Debug.Log("Invalid InputText selected.");
                    break;
            }
        }
        else
        {
            switch (focusElement)
            {

                case 1:
                    wordUsernameIndex++;
                    wordUsername = wordUsername + alphabet;
                    RegistrationScreen.Instance.usernameInput.text = wordUsername;
                    break;

                case 2:
                    wordEmaildIndex++;
                    wordEmail = wordEmail + alphabet;
                    RegistrationScreen.Instance.emailInput.text = wordEmail;
                    break;
                case 3:
                    wordPasswordIndex++;
                    wordPassword = wordPassword + alphabet;
                    RegistrationScreen.Instance.passwordInput.text = wordPassword;
                    break;
                case 4:
                    wordConfirmPasswordIndex++;
                    wordConfirmPassword = wordConfirmPassword + alphabet;
                    RegistrationScreen.Instance.confirmPasswordInput.text = wordConfirmPassword;
                    break;
                case 5:
                    wordUsernameIndexLogin++;
                    wordUsernameLogin = wordUsernameLogin + alphabet;
                    LoginScreen.Instance.usernameInput.text = wordUsernameLogin;
                    break;
                case 6:
                    wordPasswordIndexLogin++;
                    wordPasswordLogin = wordPasswordLogin + alphabet;
                    LoginScreen.Instance.passwordInput.text = wordPasswordLogin;
                    break;
                default:
                    Debug.Log("Invalid inputText selected.");
                    break;
            }
        }
    }
}
