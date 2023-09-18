using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            // Vrši se provera da li je kreirana instanca koja referencira na klasu GameManager
            if (instance == null)
                Debug.Log("GameManager is null");
            return instance;
        }
    }

    public bool isExecuted;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isExecuted = true;
    }

    void Update()
    {
        if (Quiz.Instance.isComplete && isExecuted)
        {
            isExecuted = false;
            Quiz.Instance.gameObject.SetActive(false);
            EndScreen.Instance.gameObject.SetActive(true);
            EndScreen.Instance.ShowFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
