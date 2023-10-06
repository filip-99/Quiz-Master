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

    private string userPassword;


    private void Awake()
    {
        base.Awake();

    }

    public string GetUserPassword()
    {
        return userPassword;
    }

    // Set metoda za postavljanje vrednosti userPassword.
    public void SetUserPassword(string newPassword)
    {
        userPassword = newPassword;
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

    public List<Question> GetQuestions(List<Question> originalQuestions)
    {
        List<Question> limitedQuestions = LimitQuestion(originalQuestions, 10);

        List<Question> limitedAnswers = LimitAnswers(limitedQuestions);

        List<Question> mixedQuestions = MixingAnswersAndQuestions(limitedAnswers);

        Debug.Log(mixedQuestions.Count);

        return mixedQuestions;

    }


}
