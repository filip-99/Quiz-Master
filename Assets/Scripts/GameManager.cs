using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Potrebna je promenjiva koja će uzimati vrednost najboljeg skora iz prefabs
    int highScore;
    HighscoreHandler highscoreHandler;

    void Start()
    {
        StartScreen.instance.gameObject.SetActive(true);
        // Kada se igra startuje panel sa pitanjima se aktivira
        Quiz.instance.gameObject.SetActive(false);
        // End skrin će biti isključen
        EndScreen.instance.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Quiz.instance.isComplete)
        {
            Quiz.instance.gameObject.SetActive(false);
            EndScreen.instance.gameObject.SetActive(true);
            EndScreen.instance.ShowFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
