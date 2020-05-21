using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenQuestionHolder : ContentHolder
{
    [HideInInspector] public OpenQuestion openQuestion;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI placeholderText;
    [SerializeField] TextMeshProUGUI textInput;
    [SerializeField] Button submitButton;
    UnityEvent selectEvents;

    bool done;

    protected override void Start()
    {
        base.Start();

        questionText.text = openQuestion.questionData.question;
        placeholderText.text = openQuestion.questionData.placeholder;
    }

    public void Submit()
    {
        if (!done)
        {
            openQuestion.Complete();
            done = true;
        }
    }

    public void InvokeSelectEvents()
    {
        if (!done)
            selectEvents?.Invoke();
    }

    public void SetSelectEvents(UnityEvent events)
    {
        selectEvents = events;
    }
}
