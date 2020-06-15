using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class NoteActivity : ScriptableObject
{
    [SerializeField] protected GameObject mainPrefab;
    public string title;
    [TextArea(7, 12)]
    public string fullText;
    public string paratext;
    [HideInInspector] public bool isDone;
    public UnityEvent CompleteEvents;
    public List<string> providedWords;

    public virtual GameObject GetPrefab()
    {
        return null;
    }

}

[CreateAssetMenu(fileName = "Reading", menuName = "NoteActivity/Reading")]
public class ReadingNoteActivity : NoteActivity
{
    public int readingTime;

    public override GameObject GetPrefab()
    {
        return mainPrefab;
    }
}

[CreateAssetMenu(fileName = "SimpleFillGaps", menuName = "NoteActivity/SimpleFillGaps")]
public class FillGapsNoteActivity : NoteActivity
{
    [TextArea(7, 12)]
    public string gapsText;
}

[CreateAssetMenu(fileName = "Transcription", menuName = "NoteActivity/Transcription")]
public class TranscriptionGapsNoteActivity : FillGapsNoteActivity
{
    public Sprite[] manuscripts;
}