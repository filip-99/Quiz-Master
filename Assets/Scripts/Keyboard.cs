using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    private static Keyboard instance;
    public static Keyboard Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("Keyboard is null");
            return instance;
        }
    }

    [Header("Keyboard")]
    [SerializeField]
    Button[] keyboard_part = new Button[12];

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    // Metoda za prikaz tastature
    public void ShowKeyboard()
    {
        if (StartScreen.Instance.nameInput.isFocused)
        {
            StartScreen.Instance.virtualKeyboard.SetActive(true);
            KeyboardButtons(true, true);
        }
        else if (StartScreen.Instance.emailInput.isFocused)
        {
            StartScreen.Instance.virtualKeyboard.SetActive(true);
            KeyboardButtons(true, false);
        }
    }

    //------------------------------------------------------------------------------------------------
    public void KeyboardButtons(bool check, bool name)
    {
        if (name)
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = false;
            }
        }
        else
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = true;
            }
        }
        gameObject.SetActive(check);
    }
}
