using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public int question_id { get; set; }
    public string question { get; set; }
    public List<string> answers { get; set; }
}