using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System;
using UnityEngine.EventSystems;

public class EndScreen : MonoBehaviour
{
    static EndScreen instance;
    public static EndScreen Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("EndScreen is null");
            return instance;
        }
    }

    [SerializeField]
    TextMeshProUGUI finalScoreText;

    [SerializeField]
    HighscoreHandler highscoreHandler;

    private void Awake()
    {
        instance = this;
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Čestitamo!\nVaš skor je: " + ScoreKeeper.Instance.CalculateScore() + "%";

        // Potrebna je metoda koja će uporediti postignut skor, sa najvećim skorom
        ScoreIsHigher();
    }

    private void ScoreIsHigher()
    {
        if (ScoreKeeper.Instance.CalculateScore() > 0)
        {
            highscoreHandler.SaveHighscoreData(StartScreen.Instance.nameInput.text, ScoreKeeper.Instance.CalculateScore().ToString());
        }
    }
}
