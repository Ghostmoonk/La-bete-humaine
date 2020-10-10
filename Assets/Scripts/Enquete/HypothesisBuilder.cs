using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HypothesisBuilder : MonoBehaviour
{
    Hypothesis[] hypotheses;

    [SerializeField] TMP_InputField textComponent;
    [SerializeField] DropReceiver dropReceiver;

    public UnityEvent OnTextClear;
    public UnityEvent OnWordDropped;
    List<string> droppedText;

    private void Start()
    {
        droppedText = new List<string>();
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
        if (text[text.Length - 1] != '.' && text[text.Length - 1] != '\'')
            textComponent.text += text + " ";
        else
            textComponent.text += text;

        OnWordDropped?.Invoke();
        droppedText.Add(text);
        Debug.Log(droppedText.Count);
    }

    public void ClearText()
    {
        if (textComponent.text != "")
        {
            textComponent.text = "";
            OnTextClear?.Invoke();
        }
    }

    public void ClearLastText()
    {
        if (textComponent.text != "")
        {
            int maxTextSize = textComponent.text.Length - 1;
            int compteur = maxTextSize;

            string currentString = "";
            while (currentString != droppedText[droppedText.Count - 1])
            {
                currentString = textComponent.text.Substring(compteur - 1, maxTextSize - compteur + 1);
                compteur--;
            }

            textComponent.text = textComponent.text.Substring(0, compteur);
            droppedText.RemoveAt(droppedText.Count - 1);
            OnTextClear?.Invoke();
            Debug.Log("ClearLastText");
        }
    }

    public void SubmitHypothesis()
    {
        string hypothesisToTry = "";
        if (textComponent.text.Length > 0)
            if (textComponent.text[textComponent.text.Length - 1] == ' ')
                hypothesisToTry = textComponent.text.Substring(0, textComponent.text.Length - 1);


        for (int i = 0; i < hypotheses.Length; i++)
        {
            if (hypothesisToTry == hypotheses[i].data.text && !hypotheses[i].found)
            {
                HypothesisManager.Instance.CompleteHypothesis(i);
                textComponent.text = "";
            }
        }
    }
}
