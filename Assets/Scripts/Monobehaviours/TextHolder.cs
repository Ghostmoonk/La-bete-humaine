using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//S'attache sur un text, contient simplement les informations d'un text

public class TextHolder : ContentHolder
{
    [HideInInspector] public SimpleText simpleText;
    [SerializeField] TextMeshProUGUI textMesh;
    [HideInInspector] public TextMeshProUGUI titleTextMesh;
    public bool showTitle;

    Timer lectureTimeTimer;

    private void OnEnable()
    {
        textMesh.text = simpleText.textData.content;

        if (showTitle)
        {
            titleTextMesh.text = simpleText.textData.title;
            //titleTextMesh.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(titleTextMesh.transform.parent.GetComponent<RectTransform>().sizeDelta.x, titleTextMesh.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    protected override void Start()
    {
        base.Start();
        lectureTimeTimer = GetComponent<Timer>();

        lectureTimeTimer.SetTimer(simpleText.textData.minimumReadTime);
        lectureTimeTimer.timerEndEvent.AddListener(simpleText.Complete);
        lectureTimeTimer.StartTimer();

    }

    private void Update()
    {
        if (!FindObjectOfType<ContentsSupport>().IsOnScreen() && !lectureTimeTimer.over)
        {
            lectureTimeTimer.SetTimerActive(false);
        }
        else if (FindObjectOfType<ContentsSupport>().IsOnScreen() && !lectureTimeTimer.over && !lectureTimeTimer.IsActive())
        {
            lectureTimeTimer.SetTimerActive(true);
        }
    }

    //IEnumerator WaitLectureTime()
    //{
    //    float timer = 0f;
    //    while (timer < simpleText.textData.minimumReadTime)
    //    {
    //        if (FindObjectOfType<ContentsSupport>().IsOnScreen())
    //        {
    //            timer += Time.deltaTime;
    //        }
    //        yield return null;
    //    }
    //    simpleText.Complete();
    //}
}
