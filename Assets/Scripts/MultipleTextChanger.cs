using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MultipleTextChanger : MonoBehaviour, IPointerDownHandler
{
    [TextArea(3, 5)]
    [SerializeField] string[] textArray;

    [SerializeField] TextMeshProUGUI textMesh;
    int currentIndexText = 0;

    [SerializeField] UnityEvent SentencesOver;
    [SerializeField] UnityEvent ClickEvents;

    private void OnEnable()
    {
        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (currentIndexText < textArray.Length)
        {
            textMesh.text = textArray[currentIndexText];
            currentIndexText++;
        }
        else
        {
            textMesh.text = "";
            SentencesOver?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("click");
        ClickEvents?.Invoke();
    }

}
