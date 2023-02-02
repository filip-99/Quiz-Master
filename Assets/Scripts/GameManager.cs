using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Quiz.instance.gameObject.SetActive(true);
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
