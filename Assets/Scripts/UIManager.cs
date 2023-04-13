using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public static UIManager Instance
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

        Quiz.Instance.gameObject.SetActive(false);

        EndScreen.Instance.gameObject.SetActive(false);

        RegistrationScreen.Instance.gameObject.SetActive(false);

        LoginScreen.Instance.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    // Povratak na početni ekran
    public void Back()
    {
        RegistrationScreen.Instance.gameObject.SetActive(false);
        LoginScreen.Instance.gameObject.SetActive(false);

        StartScreen.Instance.gameObject.SetActive(true);

        HideKeyboard();
    }

    // Isključivanje panela za obaveštenje
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
