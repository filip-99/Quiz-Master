using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    static ScoreKeeper instance;
    public static ScoreKeeper Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("ScoreKeeper is null");
            return instance;
        }
    }
    // Tačni odgovori igrača
    int correctAnswers = 0;
    // Broj pitanja koje je igrač video
    int questionsSeen = 0;

    void Awake()
    {
        instance = this;
    }

    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    public int GetQuestionSeen()
    {
        return questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt(correctAnswers / (float)questionsSeen * 100);
    }
}
