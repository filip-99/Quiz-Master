using IneryLibrary;
using IneryLibrary.Core;
using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using Json.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    static Quiz instance;
    public static Quiz Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("Quiz is null");
            return instance;
        }
    }

    // Potrebna je lista koja će sadržati pitanja
    List<Question> questions = new List<Question>();
    UserData userData = new UserData();

    [SerializeField] TextMeshProUGUI usernameText;

    public UserSO scriptableObject;

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    // Promenjiva će sadržati određeno pitanje iz liste
    Question currentQuestion;

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


    private async void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        questions.Clear();
        questions = BlockchainData.Instance.GetQuestions();
        userData = MediatorScript.Instance.jsonObj;
        Debug.Log(userData.username);
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
        SetUsername();
    }

    void Update()
    {
        timerImage.fillAmount = Timer.Instance.fillFraction;
        if (Timer.Instance.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            Timer.Instance.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !Timer.Instance.isAnsweringQuestion)
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
        Timer.Instance.CancelTimer();
        // Setujemo tekst za skor
        scoreText.text = "Score: " + ScoreKeeper.Instance.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.correctAnswerIndex)
        {
            questionText.text = "Exactly!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            // Kada tačno odgovorimo na pitanje povećaćemo vrednost tačnih odgovora
            ScoreKeeper.Instance.IncrementCorrectAnswers();
        }
        else
        {
            // Dakle 0 je uvek tačan odgovor
            questionText.text = "Correct answer is\n" + currentQuestion.answers[currentQuestion.correctAnswerIndex];
            // Dugme sa indeksom 0 biće tačan odgovor
            buttonImage = answerButtons[currentQuestion.correctAnswerIndex].GetComponent<Image>();
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
            ScoreKeeper.Instance.IncrementQuestionsSeen();
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
        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.answers[i];
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

    public void SetUsername()
    {
        usernameText.text = MediatorScript.Instance.jsonObj.username.ToString();
    }

}