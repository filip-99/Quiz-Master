using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

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
                default:
                    Debug.Log("Invalid inputText selected.");
                    break;
            }
        }
    }
}
