using IneryLibrary.Core.Api.v1;
using IneryLibrary.Core.Providers;
using IneryLibrary.Core;
using IneryLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Json.Lib;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class BlockchainData : Singleton<BlockchainData>
{

    public List<Question> questions = new List<Question>();
    public QuestionSO questionDataListContainer;

    public void SaveQuestions(List<Question> questions)
    {
        // ScriptableObjects
        questionDataListContainer.ClearQuestionData();
        questionDataListContainer.AddQuestionsData(questions);
        //----------------------------------------------------------------------
        // PlayerPrefs.DeleteKey("Questions");

        // Serijalizuj listu u JSON format
        // string json = JsonConvert.SerializeObject(questions);
        // Saèuvaj JSON string u PlayerPrefs
        // PlayerPrefs.SetString("Questions", json);

        // Obavezno pozovite PlayerPrefs.Save() kako biste saèuvali promene
        // PlayerPrefs.Save();

        //---------------------------------------------------------------------
    }

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
                encode_type = "string"
                // limit = 1
            });
            int index = 0;

            foreach (var row in result.rows)
            {
                questions.Add(JsonConvert.DeserializeObject<Question>(row.ToString()));
                questions[index].correctAnswer = questions[index].answers[0];
                index++;
            }

            SaveQuestions(questions);
            return questions;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private void Awake()
    {
        base.Awake();

        questions.Clear();
        GetQuestionsAsync();
    }

    public List<Question> LoadList()
    {
        // Uèitaj JSON string iz PlayerPrefs
        // string json = PlayerPrefs.GetString("Questions");

        // Deserijalizuj JSON string u listu
        // questions = JsonConvert.DeserializeObject<List<Question>>(json);

        // Debug.Log(questionDataListContainer.questionDataList.Count());

        questions = questionDataListContainer.questionDataList;

        return questions;
    }

    public List<Question> LimitQuestion(List<Question> questions, int limit)
    {
        if (questions == null)
        {
            // Ako je lista questions null, vratite praznu listu.
            return new List<Question>();
            Debug.Log("Lista je prazna");
        }

        int count = Math.Min(questions.Count, limit);

        List<Question> limitedQuestions = new List<Question>(count);

        for (int i = 0; i < count; i++)
        {
            limitedQuestions.Add(questions[i]);
        }



        return limitedQuestions;
    }

    public List<Question> LimitAnswers(List<Question> questions)
    {
        int answerCount;
        for (int i = 0; i < questions.Count; i++)
        {
            // Dobij broj elemenata u podlisti answers.
            answerCount = questions[i].answers.Count;

            // Ako je broj elemenata veæi od 4, ukloni višak elemenata.
            if (answerCount > 4)
            {
                questions[i].answers.RemoveRange(4, answerCount - 4);
            }
        }
        return questions;
    }

    private List<Question> MixingAnswersAndQuestions(List<Question> questions)
    {
        // Mešanje pitanja
        for (int i = questions.Count - 1; i >= 0; i--)
        {
            int rndQuestion = UnityEngine.Random.Range(0, i + 1);

            // Swap the questions at indices i and rndQuestion.
            Question tempQuestion = questions[i];
            questions[i] = questions[rndQuestion];
            questions[rndQuestion] = tempQuestion;

            // Mešanje odgovora u trenutnom pitanju
            for (int j = questions[i].answers.Count - 1; j >= 0; j--)
            {
                int rndAns = UnityEngine.Random.Range(0, j + 1);

                // Swap the answers at indices j and rndAns.
                string tempAnswer = questions[i].answers[j];
                questions[i].answers[j] = questions[i].answers[rndAns];
                questions[i].answers[rndAns] = tempAnswer;
            }
        }

        for (int i = 0; i < questions.Count; i++)
        {
            for (int j = 0; j < questions[i].answers.Count; j++)
            {
                if (questions[i].correctAnswer == questions[i].answers[j])
                {
                    questions[i].correctAnswerIndex = j;
                }
            }
        }

        return questions;
    }

    public List<Question> GetQuestions()
    {

        List<Question> originalQuestions = questions;

        List<Question> limitedQuestions = LimitQuestion(originalQuestions, 10);

        List<Question> limitedAnswers = LimitAnswers(limitedQuestions);

        List<Question> mixedQuestions = MixingAnswersAndQuestions(limitedAnswers);

        Debug.Log(mixedQuestions.Count);

        return mixedQuestions;

    }


}
