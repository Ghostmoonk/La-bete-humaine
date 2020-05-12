using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;

//S'attache sur un text, contient simplement les informations d'un text

public class TextHolder : MonoBehaviour
{
    [HideInInspector] public SimpleText simpleText;

    [SerializeField] TextMeshProUGUI textMesh;

    private void OnEnable()
    {
        textMesh.text = simpleText.textData.content;
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
