using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary;
using System;
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
    [SerializeField] TextMeshProUGUI usernameText;


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

    void Start()
    {
        GetQuestions();
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

    // ---------------------------------------------------------------------------

    public async void GetQuestions()
    {

        Inery inery = new Inery(new IneryConfigurator()
        {
            HttpEndpoint = "https://tas.blockchain-servers.world", //Mainnet
            ChainId = "3b891f1a78a7c27cf5dbaa82d2f30f96d0452262a354b4995b88c162ab066eee",
            ExpireSeconds = 60,
            SignProvider = new DefaultSignProvider("5KftZF2nz6eiYy9ZBtGymj75XJWiKJk2f859qdc6kGGMb6boAkb")
        });

        try
        {
            var result = await inery.GetTableRows<Question>(new GetTableRowsRequest()
            {
                json = true,
                code = "quiz",
                scope = "quiz",
                table = "questions"
            });
            Debug.Log(result.rows[0].ToString()); // poruka
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    // ---------------------------------------------------------------------------

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
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Tačno!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            // Kada tačno odgovorimo na pitanje povećaćemo vrednost tačnih odgovora
            ScoreKeeper.Instance.IncrementCorrectAnswers();
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

    // Metoda za setovanje usernamea
    public void SetUsername(string username)
    {
        usernameText.text = username;
    }

}