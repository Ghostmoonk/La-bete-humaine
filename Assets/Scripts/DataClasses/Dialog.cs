using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    Sentence rootSentence;
    public Sentence currentSentence;

    public Dialog(Sentence rootSentence)
    {
        this.rootSentence = rootSentence;
        currentSentence = rootSentence;
    }

    public void SetNextSentence()
    {
        currentSentence = currentSentence.GetNextSentence();
    }

    public void SetNextSentence(Sentence newSentence)
    {
        currentSentence = newSentence;
    }

    public void Reset()
    {
        currentSentence = rootSentence;
    }
}

public class Sentence
{
    public string characterName;
    public string content;
    private Sentence nextSentence;

    public Sentence(string name, string text)
    {
        characterName = name;
        content = text;
    }

    public void SetNextSentence(Sentence sentence)
    {
        nextSentence = sentence;
    }

    public Sentence GetNextSentence()
    {
        return nextSentence;
    }

    public void ShowSentences()
    {
        //Debug.Log(content);
        if (nextSentence != null)
            nextSentence.ShowSentences();
    }
}

public class QuestioningSentence : Sentence
{
    public Answer[] answers;

    public QuestioningSentence(string name, string content, Answer[] answers) : base(name, content)
    {
        this.answers = answers;
    }
}

public class Answer
{
    public string answerText;
    public Sentence sentence;

    public Answer(string answerText, Sentence sentence)
    {
        this.answerText = answerText;
        this.sentence = sentence;
    }
}

