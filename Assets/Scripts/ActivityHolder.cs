using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Parent of all note activity (Reading, FillGaps, Translation ...)
public abstract class ActivityHolder : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI contentTextMesh;
    [SerializeField] protected TextMeshProUGUI paratextTextMesh;
    [SerializeField] UnityEvent ActivityRelatedCompleteEvent;
    protected UnityEvent GeneralCompleteEvent;

    public NoteActivity noteActivity;
    [HideInInspector] public bool isDone;

    protected NotesDisplayer notesDisplayer;

    //Base : set text in components
    public virtual void SetContent(NoteActivityEvent activityData)
    {
        contentTextMesh.text = activityData.noteActivity.fullText;
        paratextTextMesh.text = activityData.noteActivity.paratext;
        noteActivity = activityData.noteActivity;

        //ActivityRelatedCompleteEvent.AddListener(activityData.GeneralCompleteEvents.Invoke);
        GeneralCompleteEvent = activityData.GeneralCompleteEvents;
    }

    protected virtual void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        notesDisplayer = gameObject.GetComponentInParent<NotesDisplayer>();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(WaitResize());
    }

    //Need to be resized after end of frame otherwise it won't have time to know its height
    IEnumerator WaitResize()
    {
        yield return new WaitForEndOfFrame();
        Resizer.ResizeHeight(GetComponent<RectTransform>());
    }

    public virtual void CompleteActivity()
    {
        isDone = true;
        ActivityRelatedCompleteEvent?.Invoke();
        GeneralCompleteEvent?.Invoke();
    }

    public virtual void ProvideWords()
    {
        notesDisplayer.wordSorter.RevealWords(noteActivity.providedWords);
    }
}
