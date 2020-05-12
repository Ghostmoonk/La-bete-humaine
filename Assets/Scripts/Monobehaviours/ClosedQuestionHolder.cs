using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ClosedQuestionHolder : MonoBehaviour
{
    [HideInInspector] public ClosedQuestion closedQuestion;

    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Transform optionsContainer;
    [SerializeField] GameObject optionPrefab;

    private void Start()
    {
        questionText.text = closedQuestion.questionData.question;

        InstantiateOptions();
    }

    private void InstantiateOptions()
    {
        foreach (var answer in closedQuestion.questionData.answersRefDico.Values.Distinct())
        {
            GameObject answerOption = Instantiate(optionPrefab, optionsContainer);
            answerOption.GetComponentInChildren<TextMeshProUGUI>().text = answer;

            foreach (var item in closedQuestion.questionData.answersRefDico)
            {
                if (item.Value == answer)
                    Debug.Log(item.Key);
            }

        }
    }
}
