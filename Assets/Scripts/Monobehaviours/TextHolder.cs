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
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI paratextTextMesh;
    [SerializeField] private ManuscriptToggler manuscriptToggler;

    Timer lectureTimeTimer;

    private void OnEnable()
    {
        textMesh.text = simpleText.textData.content;

        if (simpleText.textData.title.Length > 0)
            titleTextMesh.text = simpleText.textData.title;
        else
            titleTextMesh.gameObject.SetActive(false);

        if (simpleText.textData.paratext.Length > 0)
            paratextTextMesh.text = simpleText.textData.paratext;

        else
        {
            paratextTextMesh.gameObject.SetActive(false);

            if (simpleText.textData.manuscritPath != null)
                manuscriptToggler.transform.SetParent(textMesh.transform);
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
    //À factoriser avec ImageHolder
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
}
