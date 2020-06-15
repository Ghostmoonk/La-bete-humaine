using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class ReadingActivityHolder : ActivityHolder
{
    ReadingNoteActivity readingNoteActivity;
    Timer timer;

    private void OnEnable()
    {
        if (timer != null)
        {
            timer.SetTimer(readingNoteActivity.readingTime - timer.GetTimePassed());
            timer.StartTimer();
            timer.active = true;
        }
    }
    public override void SetContent(NoteActivity noteActivity)
    {
        base.SetContent(noteActivity);
        readingNoteActivity = (ReadingNoteActivity)noteActivity;

        timer = GetComponent<Timer>();
    }

    private void OnDisable()
    {
        timer.active = false;
    }

    public void ProvideWords()
    {
        //TODO: Ajouter les mots à une liste pour l'hypothèse
        Debug.Log("Provide words");
    }

    public override void CompleteActivity()
    {
        ProvideWords();

        readingNoteActivity.CompleteEvents?.Invoke();
    }
}
