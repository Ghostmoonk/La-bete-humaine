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
    public new OpenQuestionData questionData;

    public OpenQuestion(QuestionData data) : base(data)
    {
        questionData = (OpenQuestionData)data;
    }

    public Vector2 textAreaSize;

}

public sealed class ClosedQuestion : Question
{
    public ClosedQuestion(QuestionData data) : base(data)
    {
        questionData = (ClosedQuestionData)data;
    }

    public Dictionary<int, Answer> possibilities;

    public struct Answer
    {
        string proposition;
        string justification;
    }
}