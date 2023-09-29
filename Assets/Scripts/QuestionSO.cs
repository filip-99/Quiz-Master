using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Questions", fileName = "QuizQuestion")]
public class QuestionSO : ScriptableObject
{
    public List<Question> questionDataList = new List<Question>();

    // Dodavanje više pitanja
    public void AddQuestionsData(List<Question> newQuestion)
    {
        questionDataList = newQuestion;
    }

    // Dodavanje jednog pitanja
    public void AddUserData(Question newQuestion)
    {
        questionDataList.Add(newQuestion);
    }

    public void RemoveQuestionData(Question data)
    {
        questionDataList.Remove(data);
    }

    public void ClearQuestionData()
    {
        questionDataList.Clear();
    }
}
