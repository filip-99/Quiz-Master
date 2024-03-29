﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public static EndScreen instance;

    [SerializeField] TextMeshProUGUI finalScoreText;


    private void Awake()
    {
        instance = this;
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Čestitamo!\nVaš skor je: " + ScoreKeeper.instance.CalculateScore() + "%";
    }

}
