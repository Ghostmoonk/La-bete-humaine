using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenQuestionHolder : MonoBehaviour
{
    [HideInInspector] public OpenQuestion openQuestion;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI placeholderText;
    [SerializeField] TextMeshProUGUI textInput;
    [SerializeField] Button submitButton;

    private void Start()
    {
        Debug.Log(openQuestion);
        questionText.text = openQuestion.questionData.question;
        placeholderText.text = openQuestion.questionData.placeholder;
    }

    public void Submit()
    {
        openQuestion.Complete();
    }
}
