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
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
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
    string serializedData;

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    // Promenjiva će sadržati određeno pitanje iz liste
    Question currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;
    // Pošto je prvi odgovor u listi uvek tačan, potrebna je promenjiva koja će da ga sačuva
    int correctAnsware;

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

    // ---------------------------------------------------------------------------

    public async Task<List<Question>> GetQuestionsAsync()
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
            var result = await inery.GetTableRows(new GetTableRowsRequest()
            {
                json = true,
                code = "quiz",
                scope = "quiz",
                table = "questions",
                index_position = "0",
                key_type = "i64",
                encode_type = "string",
                limit = 1
            });
            int i = 0;

            foreach (var row in result.rows)
            {
                questions.Add(JsonConvert.DeserializeObject<Question>(row.ToString()));
                questions[i].correctAnswer = questions[i].answers[0];
                i++;
            }

            i = 0;
            // Mešanje pitanja u listi
            for (i = questions.Count - 1; i >= 0; i--)
            {
                int rnd = UnityEngine.Random.Range(0, i);

                // Swap the elements at indices i and rnd.
                Question temp = questions[i];
                questions[i] = questions[rnd];
                questions[rnd] = temp;
            }

            i = 0;
            // Mešanje pitanja u listi
            for (i = questions.Count - 1; i >= 0; i--)
            {
                for (int j = questions[i].answers.Count - 1; j > 0; j--)
                {
                    int rndAns = Random.Range(0, j);
                    // Debug.Log(rndAns);

                    // Swap the elements at indices i and rnd.
                    string temp = questions[i].answers[j];
                    questions[i].answers[j] = questions[i].answers[rndAns];
                    questions[i].answers[rndAns] = temp;
                }

            }


            return questions;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }
    // ---------------------------------------------------------------------------

    private async void Awake()
    {
        instance = this;
        questions.Clear();
        questions = await GetQuestionsAsync();
        // Debug.Log(questions.Count);
    }

    private async void Start()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;

        string serializedData = JsonConvert.SerializeObject(questions);
        PlayerPrefs.SetString("questionsList", serializedData);
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
        Debug.Log(questions.Count);
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        Timer.Instance.CancelTimer();
        // Setujemo tekst za skor
        scoreText.text = "Score: " + ScoreKeeper.Instance.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        string serializedData = PlayerPrefs.GetString("questionsList");
        List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(serializedData);

        Debug.Log(questions.Count);

        Image buttonImage;
        // 0 - Svako pitanje sa indeksom 0 je tačan odgovor
        for (int i = 0; i < questions.Count; i++)
        {
            Debug.Log("Prvi for");

            for (int j = 0; j < questions[i].answers[j].Count(); j++)
            {
                Debug.Log("Drugi for");
                if (questions[i].correctAnswer.Equals(questions[i].answers[j]))
                {
                    correctAnsware = j;
                    Debug.Log("Korektan odgovor je: " + j);
                }
            }
        }

        if (index == correctAnsware)
        {
            questionText.text = "Tačno!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            // Kada tačno odgovorimo na pitanje povećaćemo vrednost tačnih odgovora
            ScoreKeeper.Instance.IncrementCorrectAnswers();
        }
        else
        {
            // Dakle 0 je uvek tačan odgovor
            questionText.text = "Tačan odgovor je\n" + currentQuestion.answers[correctAnsware];
            // Dugme sa indeksom 0 biće tačan odgovor
            buttonImage = answerButtons[0].GetComponent<Image>();
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
        questionText.text = currentQuestion.question.ToString();

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

    // Metoda za setovanje usernamea
    public void SetUsername(string username)
    {
        usernameText.text = username;
    }

}