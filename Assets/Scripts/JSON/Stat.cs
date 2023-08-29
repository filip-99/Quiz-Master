using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fields
{
    public string name = "questions";
    public string type = "uint64";
}

public class Question
{
    public string name = "questions";
    public string bas = "";
    public string fields = "";
    public List<Fields> fieldsList = new List<Fields>();
}
