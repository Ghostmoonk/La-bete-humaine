using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class Question : Content
{
    public QuestionData questionData;

    public Question(QuestionData data)
    {
        questionData = data;
    }
}

public sealed class OpenQuestion : Question
{
    public OpenQuestion(QuestionData data) : base(data)
    {
    }
    public Vector2 textAreaSize;

}

public sealed class ClosedQuestion : Question
{
    public ClosedQuestion(QuestionData data) : base(data)
    {
    }

    public Dictionary<int, Answer> possibilities;

    public struct Answer
    {
        string proposition;
        string justification;
    }
}