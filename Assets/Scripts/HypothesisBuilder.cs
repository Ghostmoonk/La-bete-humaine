using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HypothesisBuilder : MonoBehaviour
{
    [SerializeField] Hypothesis[] hypotheses;

    [SerializeField] TMP_InputField textComponent;
    [SerializeField] DropReceiver dropReceiver;

    private void Start()
    {
        dropReceiver.OnDropEvents += AppendText;

        int longestHypothesisLength = 0;

        foreach (Hypothesis item in hypotheses)
        {
            if (item.text.Length > longestHypothesisLength)
                longestHypothesisLength = item.text.Length;
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
}
