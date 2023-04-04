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
        if (StartScreen.Instance.nameInput.isFocused)
        {
            focusElement = 1;
        }
        else if (StartScreen.Instance.emailInput.isFocused)
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
                StartScreen.Instance.nameInput.text = wordName.ToString();
            }
            else if (focusElement == 2)
            {
                if (alphabet == "bs")
                {
                    wordEmailIndex--;
                    if (wordEmail.Length > 0)
                        wordEmail = wordEmail.Remove(wordEmail.Length - 1, 1);
                    StartScreen.Instance.emailInput.text = wordEmail;
                }
            }
        }
        else
        {
            if (focusElement == 1)
            {
                wordNameIndex++;
                wordName = wordName + alphabet;
                StartScreen.Instance.nameInput.text = wordName;
            }
            else if (focusElement == 2)
            {
                wordEmailIndex++;
                wordEmail = wordEmail + alphabet;
                StartScreen.Instance.emailInput.text = wordEmail;
            }
        }
    }
}
