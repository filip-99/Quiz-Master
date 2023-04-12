using System;
using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        instance = this;
    }

    // Potrebna je promenjiva koja će uzimati vrednost najboljeg skora iz prefabs
    int highScore;

    void Update()
    {
        if (Quiz.Instance.isComplete)
        {
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
