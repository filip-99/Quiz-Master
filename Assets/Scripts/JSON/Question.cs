using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public List<string> answers { get; set; }
    public string question { get; set; }
    public int question_id { get; set; }
    public string correctAnswer { get; set; }
    public int correctAnswerIndex { get; set; }

}