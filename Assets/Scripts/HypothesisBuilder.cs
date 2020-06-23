﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HypothesisBuilder : MonoBehaviour
{
    Hypothesis[] hypotheses;

    [SerializeField] TMP_InputField textComponent;
    [SerializeField] DropReceiver dropReceiver;

    private void Start()
    {
        dropReceiver.OnDropEvents += AppendText;
        hypotheses = HypothesisManager.Instance.GetHypothesis();

        int longestHypothesisLength = 0;

        foreach (Hypothesis item in hypotheses)
        {
            if (item.data.text.Length > longestHypothesisLength)
                longestHypothesisLength = item.data.text.Length;
        }
        textComponent.characterLimit = longestHypothesisLength;
    }
    public void AppendText(object sender, string text)
    {
        if (text[text.Length - 1] != '.')
            textComponent.text += text + " ";
        else
            textComponent.text += text;
    }

    public void ClearText()
    {
        textComponent.text = "";
    }

    public void SubmitHypothesis()
    {
        if (textComponent.text.Length > 0)
            if (textComponent.text[textComponent.text.Length - 1] == ' ')
                textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 1);


        for (int i = 0; i < hypotheses.Length; i++)
        {
            Debug.Log(hypotheses[i].found);
            if (textComponent.text == hypotheses[i].data.text && !hypotheses[i].found)
            {
                HypothesisManager.Instance.CompleteHypothesis(i);
                textComponent.text = "";
            }
        }
    }
}