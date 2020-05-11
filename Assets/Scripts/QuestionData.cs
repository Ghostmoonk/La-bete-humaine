using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionData
{
    public bool open = false;
    public string question;
    public Dictionary<int, string> answersRefDico;

    public QuestionData(string question, Dictionary<int, string> answersRefDico = null)
    {
        this.question = question;
        if (answersRefDico == null)
            open = true;
        else
            this.answersRefDico = answersRefDico;

    }
}
