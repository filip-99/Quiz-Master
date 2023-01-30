using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool isAnsweringQuestion = false;

    float timerValue;

    void Update()
    {

    }

    private void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (timerValue <= 0)
            {
                isAnsweringQuestion = true;
                timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (timerValue <= 0)
            {
                isAnsweringQuestion = false;
                timerValue = timeToCompleteQuestion;
            }
        }
    }
}
