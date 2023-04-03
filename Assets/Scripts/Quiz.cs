using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public static Quiz instance;

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    // Potrebna je lista koja će sadržati pitanja
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    // Promenjiva će sadržati određeno pitanje iz liste
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;

    [Header("Scoring Data")]
    [SerializeField] TextMeshProUGUI scoreText;


    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;


    private void Awake()
    {
        instance = this;
        // Progres bar zavisi od broja pitanja, pa je jednak ukupnom broju
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;

    }


    void Update()
    {
        timerImage.fillAmount = Timer.instance.fillFraction;
        if (Timer.instance.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            Timer.instance.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !Timer.instance.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        Timer.instance.CancelTimer();
        // Setujemo tekst za skor
        scoreText.text = "Score: " + ScoreKeeper.instance.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Tačno!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            // Kada tačno odgovorimo na pitanje povećaćemo vrednost tačnih odgovora
            ScoreKeeper.instance.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "Tačan odgovor je\n" + currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex());
            buttonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            ScoreKeeper.instance.IncrementQuestionsSeen();
        }
    }

    // Generisanje slučajnog odgovora
    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        // U koliko pitanje postoji u listi ukloni ga
        if (questions.Contains(currentQuestion))
            questions.Remove(currentQuestion);
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Button>().interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

}
