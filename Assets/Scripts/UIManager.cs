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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        Quiz.Instance.gameObject.SetActive(true);

        EndScreen.Instance.gameObject.SetActive(false);
    }
}
