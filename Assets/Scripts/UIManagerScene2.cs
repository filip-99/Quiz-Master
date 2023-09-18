using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScene2 : MonoBehaviour
{
    private static UIManagerScene2 instance;
    public static UIManagerScene2 Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("UIManager is null");
            return instance;
        }
    }

    [Header("MessagePanel")]
    public GameObject messagePanel;
    public TextMeshProUGUI message;
    public Button cancelButton;

    [Header("Keyboard")]
    public GameObject virtualKeyboard;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartScreen.Instance.gameObject.SetActive(true);

        RegistrationScreen.Instance.gameObject.SetActive(false);

        LoginScreen.Instance.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    // Povratak na poèetni ekran
    public void Back()
    {
        RegistrationScreen.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(false);

        StartScreen.Instance.gameObject.SetActive(true);

        HideKeyboard();
    }

    // Iskljuèivanje panela za obaveštenje
    public void ClickCancel()
    {
        messagePanel.SetActive(false);
    }

    // Metoda za prikaz/sakrivanje tastature
    public void ShowKeyboard(Transform transform)
    {
        virtualKeyboard.SetActive(true);
        virtualKeyboard.transform.SetParent(transform);
    }

    public void HideKeyboard()
    {
        virtualKeyboard.transform.SetParent(null);
        virtualKeyboard.SetActive(false);
    }

    public void ShowPanel(Transform transform)
    {
        messagePanel.SetActive(true);
        messagePanel.transform.SetParent(transform);
    }

    public void HidePanel(Transform transform)
    {
        messagePanel.transform.SetParent(null);
        messagePanel.SetActive(false);
    }
}
