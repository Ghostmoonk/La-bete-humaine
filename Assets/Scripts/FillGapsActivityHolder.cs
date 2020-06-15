using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGapsActivityHolder : ActivityHolder
{
    FillGapsNoteActivity readingNoteActivity;
    [SerializeField] GameObject inputFieldPrefab;
    public override void SetContent(NoteActivity noteActivity)
    {
        base.SetContent(noteActivity);
        readingNoteActivity = (FillGapsNoteActivity)noteActivity;
    }

    public override void CompleteActivity()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetUpInputFields()
    {
        for (int i = 0; i < contentTextMesh.textInfo.characterCount; i++)
        {
            //if(contentTextMesh.textInfo.)
        }
    }
}
