using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    static Timer instance;
    public static Timer Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("Timer is null");
            return instance;
        }
    }

    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool isAnsweringQuestion = false;
    // Potrebno je izračunati
    public float fillFraction;
    public bool loadNextQuestion;

    float timerValue;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Quiz.Instance.gameObject.activeSelf == true)
            UpdateTimer();
    }

    private void UpdateTimer()
    {
        // Oduzimamo vreme koje je uvek isto na svim računarima
        timerValue -= Time.deltaTime;

        // U koliko je dat odgovor uslov će biti ispunjen
        if (isAnsweringQuestion)
        {
            // U koliko je vreme veće od 0
            if (timerValue > 0)
            {
                // 
                fillFraction = timerValue / timeToCompleteQuestion; // 10/10=1 || 5/10=0.5
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }
}
