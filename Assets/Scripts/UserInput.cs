using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

    string wordName = null;
    string wordEmail = null;
    int wordNameIndex = 0;
    int wordEmailIndex = 0;
    string alpha;
    int focusElement;

    void Update()
    {
        if (RegistrationScreen.Instance.usernameInput.isFocused)
        {
            focusElement = 1;
        }
        else if (RegistrationScreen.Instance.usernameInput.isFocused)
        {
            focusElement = 2;
        }
    }

    public void AlphabetFunction(string alphabet)
    {

        if (alphabet == "bs")
        {
            if (focusElement == 1)
            {
                wordNameIndex--;
                if (wordName.Length > 0)
                    wordName = wordName.Remove(wordName.Length - 1, 1);
                RegistrationScreen.Instance.usernameInput.text = wordName.ToString();
            }
            else if (focusElement == 2)
            {
                if (alphabet == "bs")
                {
                    wordEmailIndex--;
                    if (wordEmail.Length > 0)
                        wordEmail = wordEmail.Remove(wordEmail.Length - 1, 1);
                    RegistrationScreen.Instance.usernameInput.text = wordEmail;
                }
            }
        }
        else
        {
            if (focusElement == 1)
            {
                wordNameIndex++;
                wordName = wordName + alphabet;
                RegistrationScreen.Instance.usernameInput.text = wordName;
            }
            else if (focusElement == 2)
            {
                wordEmailIndex++;
                wordEmail = wordEmail + alphabet;
                RegistrationScreen.Instance.usernameInput.text = wordEmail;
            }
        }
    }
}
