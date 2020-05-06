using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : Content
{
    public string question;
}

public sealed class OpenQuestion : Question
{
    public Vector2 textAreaSize;
}

public sealed class ClosedQuestion : Question
{
    public Dictionary<int, Answer> possibilities;

    public struct Answer
    {
        string proposition;
        string justification;
    }
}