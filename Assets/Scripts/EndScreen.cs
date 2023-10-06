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

    private void Awake()
    {
        instance = this;
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Congratulations!\nYour score is: " + ScoreKeeper.Instance.CalculateScore() + "%";
        if (ScoreKeeper.Instance.CalculateScore() > 0)
        {
            Debug.Log(ScoreKeeper.Instance.CalculateScore());
            GetPostDataServer.Instance.SetScoreUser(GetPostDataServer.Instance.GetUserData().username, BlockchainData.Instance.GetUserPassword(), ScoreKeeper.Instance.CalculateScore().ToString());
        }
    }
}
