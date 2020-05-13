using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//S'attache sur un text, contient simplement les informations d'un text

public class TextHolder : MonoBehaviour
{
    [HideInInspector] public SimpleText simpleText;

    [SerializeField] TextMeshProUGUI textMesh;
    public bool showTitle;
    [HideInInspector] public TextMeshProUGUI titleTextMesh;

    private void OnEnable()
    {
        textMesh.text = simpleText.textData.content;

        if (showTitle)
        {
            titleTextMesh.text = simpleText.textData.title;
            //titleTextMesh.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(titleTextMesh.transform.parent.GetComponent<RectTransform>().sizeDelta.x, titleTextMesh.GetComponent<RectTransform>().sizeDelta.y);
        }
        StartCoroutine(nameof(WaitLectureTime));

        //simpleText.AddObserver(FindObjectOfType<ContentsSupport>().contentDisplayer);
    }

    IEnumerator WaitLectureTime()
    {
        float timer = 0f;
        while (timer < simpleText.textData.minimumReadTime)
        {
            if (FindObjectOfType<ContentsSupport>().IsOnScreen())
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        simpleText.Complete();
    }
}
