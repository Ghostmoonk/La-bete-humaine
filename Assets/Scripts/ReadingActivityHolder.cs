using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class ReadingActivityHolder : ActivityHolder
{
    protected new ReadingNoteActivity noteActivity;

    Timer timer;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (timer != null)
        {
            timer.SetTimer(noteActivity.readingTime - timer.GetTimePassed());
            timer.StartTimer();
            timer.active = true;
        }
    }
    public override void SetContent(NoteActivityEvent noteActivity)
    {
        base.SetContent(noteActivity);
        this.noteActivity = (ReadingNoteActivity)noteActivity.noteActivity;

        timer = GetComponent<Timer>();
    }

    private void OnDisable()
    {
        timer.active = false;
    }

    public override void CompleteActivity()
    {
        base.CompleteActivity();

    }
}
