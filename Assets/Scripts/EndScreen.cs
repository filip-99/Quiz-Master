using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] TextMeshProUGUI finalScoreText;


    private void Awake()
    {
        instance = this;
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Čestitamo!\nVaš skor je: " + ScoreKeeper.Instance.CalculateScore() + "%";
    }

}
