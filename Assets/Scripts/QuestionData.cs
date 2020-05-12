using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestionData
{
    public string question;
}

[Serializable]
public class OpenQuestionData : QuestionData
{
    public string placeholder;

    public OpenQuestionData(string question, string placeholder)
    {
        this.question = question;
        this.placeholder = placeholder;

    }
}

//Question fermée
[Serializable]
public class ClosedQuestionData : QuestionData
{
    public Dictionary<int, string> answersRefDico;

    //Question ouverte
    public ClosedQuestionData(string question, Dictionary<int, string> answersRefDico)
    {
        this.question = question;
        this.answersRefDico = answersRefDico;
    }
    //Question fermée

}
