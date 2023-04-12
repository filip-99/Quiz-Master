using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private static StartScreen instance;
    public static StartScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("StartScreen is null");
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Login()
    {
        gameObject.SetActive(false);
        Quiz.Instance.gameObject.SetActive(false);
        RegistrationScreen.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(true);

        UIManager.Instance.ShowKeyboard(LoginScreen.Instance.transform);
    }

    public void Registration()
    {
        gameObject.SetActive(false);
        Quiz.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(false);
        RegistrationScreen.Instance.gameObject.SetActive(true);

        UIManager.Instance.ShowKeyboard(RegistrationScreen.Instance.transform);
    }
}
