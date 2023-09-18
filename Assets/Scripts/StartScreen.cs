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
        RegistrationScreen.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(true);

        UIManagerScene2.Instance.ShowKeyboard(LoginScreen.Instance.gameObject.transform);
    }

    public void Registration()
    {
        gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(false);
        RegistrationScreen.Instance.gameObject.SetActive(true);
        UIManagerScene2.Instance.ShowKeyboard(RegistrationScreen.Instance.gameObject.transform);
    }
}
