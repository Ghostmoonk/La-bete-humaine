using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public struct NoteActivityEvent
{
    public NoteActivity noteActivity;
    public UnityEvent GeneralCompleteEvents;
}

//At beginning, instantiate all notes
//Manage their display
public class NotesDisplayer : MonoBehaviour
{
    [Header("Support")]
    [SerializeField] Transform manuscriptContainer;
    [SerializeField] Transform exerciceContainer;

    [Header("Navigation")]
    [SerializeField] Button previousNoteButton;
    [SerializeField] Button previousNoteButtonRightPage;
    [SerializeField] Button nextNoteButton;

    [Header("Content")]
    [SerializeField] TextMeshProUGUI titleMesh;
    string summaryTitle;
    [SerializeField] GameObject summaryText;
    [SerializeField] GameObject summaryList;
    [SerializeField] Transform suummaryListContainer;
    [SerializeField] GameObject summaryNoteTextPrefab;
    [SerializeField] Image exerciceIconImage;
    public WordSorter wordSorter;
    [SerializeField] List<NoteActivityEvent> notesActivities;

    [HideInInspector] public NoteActivity currentNote;

    [Header("Audio")]
    [SerializeField] AudioSource bookSource;

    [Header("Events")]
    public UnityEvent DisplayNoteWithoutManuscript;
    public UnityEvent DisplayNoteWithManuscript;

    //Keep all Activity Holders
    private List<ActivityHolder> activityHolders;
    private List<ManuscriptActivityHolder[]> manuscriptHolders;

    private int currentNoteIndex = -1;

    private void Start()
    {
        activityHolders = new List<ActivityHolder>();
        manuscriptHolders = new List<ManuscriptActivityHolder[]>();
        summaryTitle = titleMesh.text;

        int count = 0;
        foreach (NoteActivityEvent note in notesActivities)
        {
            InstantiateSummary(note.noteActivity, count);
            InstantiateActivity(note);
            count++;
        }

        DisplayActivity(currentNoteIndex);

        LayoutRebuilder.ForceRebuildLayoutImmediate(summaryText.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(summaryList.GetComponent<RectTransform>());
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySound(bookSource, "open-book");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySound(bookSource, "close-book");
    }

    private void InstantiateSummary(NoteActivity note, int index)
    {
        GameObject currentSummaryText = Instantiate(summaryNoteTextPrefab, suummaryListContainer);
        currentSummaryText.GetComponentInChildren<TextMeshProUGUI>().text = note.title;
        currentSummaryText.GetComponent<Button>().onClick.AddListener(() => DisplayActivity(index));
    }

    public List<NoteActivity> GetNotes()
    {
        List<NoteActivity> notes = new List<NoteActivity>();
        for (int i = 0; i < notesActivities.Count; i++)
        {
            notes.Add(notesActivities[i].noteActivity);
        }
        return notes;
    }

    private void InstantiateActivity(NoteActivityEvent note)
    {
        GameObject noteToInstantiate = Instantiate(note.noteActivity.GetExercicePrefab(), exerciceContainer);

        if (note.noteActivity.manuscripts.Length > 0)
        {
            ManuscriptActivityHolder[] manuscripts = new ManuscriptActivityHolder[note.noteActivity.manuscripts.Length];
            for (int i = 0; i < note.noteActivity.manuscripts.Length; i++)
            {
                GameObject manuscriptToInstantiate = Instantiate(note.noteActivity.GetManuscriptPrefab(), manuscriptContainer);
                manuscripts[i] = manuscriptToInstantiate.GetComponent<ManuscriptActivityHolder>();
                manuscripts[i].SetContent(note.noteActivity.manuscripts[i].sprite, note.noteActivity.manuscripts[i].paratext);
                manuscriptToInstantiate.SetActive(false);

            }
            manuscriptHolders.Add(manuscripts);
        }
        else
        {
            manuscriptHolders.Add(null);
        }

        if (noteToInstantiate.GetComponent<ActivityHolder>())
        {
            activityHolders.Add(noteToInstantiate.GetComponent<ActivityHolder>());
            noteToInstantiate.GetComponent<ActivityHolder>().SetContent(note);

            noteToInstantiate.SetActive(false);
        }
        else
            throw new Exception("Le prefab à instancier :" + currentNote.GetExercicePrefab().name + ", n'a pas de composant ActivityHolder");

    }

    public void DisplayActivity(int index)
    {
        if (index > currentNoteIndex)
            SoundManager.Instance.PlaySound(bookSource, "next-page");
        else if (index < currentNoteIndex)
            SoundManager.Instance.PlaySound(bookSource, "previous-page");

        //Desactivate the current content
        if (index != currentNoteIndex && currentNoteIndex >= 0)
            activityHolders[currentNoteIndex].gameObject.SetActive(false);

        else if (index != currentNoteIndex && currentNoteIndex == -1)
        {
            summaryText.SetActive(false);
            summaryList.SetActive(false);
        }
        else
            return;

        if (currentNoteIndex > 0)
            if (manuscriptHolders[currentNoteIndex] != null)
            {
                for (int i = 0; i < manuscriptHolders[currentNoteIndex].Length; i++)
                {
                    manuscriptHolders[currentNoteIndex][i].gameObject.SetActive(false);
                }
            }

        if (index < notesActivities.Count && index >= 0)
        {
            currentNote = notesActivities[index].noteActivity;
            exerciceIconImage.sprite = activityHolders[index].exerciceTypeIcon;

            if (!exerciceIconImage.gameObject.activeSelf)
                exerciceIconImage.gameObject.SetActive(true);

            titleMesh.text = "Note n°" + (index + 1) + " : " + currentNote.title;

            activityHolders[index].gameObject.SetActive(true);

            if (manuscriptHolders[index] != null)
            {
                if (currentNoteIndex >= 0)
                    if (manuscriptHolders[currentNoteIndex] == null)
                        DisplayNoteWithManuscript?.Invoke();

                for (int i = 0; i < manuscriptHolders[index].Length; i++)
                {
                    manuscriptHolders[index][i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (currentNoteIndex == -1)
                    DisplayNoteWithoutManuscript?.Invoke();
                else if (manuscriptHolders[currentNoteIndex] != null)
                    DisplayNoteWithoutManuscript?.Invoke();
            }

            if (index == notesActivities.Count - 1)
            {
                ToggleButtonInteractable(nextNoteButton, false);
                ToggleButtonInteractable(previousNoteButton, true);
                ToggleButtonInteractable(previousNoteButtonRightPage, true);
            }
            else if (index == 0)
            {
                ToggleButtonInteractable(nextNoteButton, true);
                ToggleButtonInteractable(previousNoteButton, true);
                ToggleButtonInteractable(previousNoteButtonRightPage, true);
                summaryText.SetActive(false);
                summaryList.SetActive(false);
            }
            else
            {
                ToggleButtonInteractable(nextNoteButton, true);
                ToggleButtonInteractable(previousNoteButton, true);
                ToggleButtonInteractable(previousNoteButtonRightPage, true);
            }

        }
        else if (index == -1)
        {
            if (manuscriptHolders[currentNoteIndex] == null)
            {
                DisplayNoteWithManuscript?.Invoke();
            }
            exerciceIconImage.gameObject.SetActive(false);
            ToggleButtonInteractable(previousNoteButton, false);
            ToggleButtonInteractable(previousNoteButtonRightPage, false);
            //Afficher un sommaire
            summaryText.SetActive(true);
            summaryList.SetActive(true);
            titleMesh.text = summaryTitle;
        }
        currentNoteIndex = index;
    }

    private void ToggleButtonInteractable(Button button, bool interactable)
    {
        button.interactable = interactable;
    }

    //private void RemoveChilds(Transform parent)
    //{
    //    for (int i = 0; i < parent.childCount; i++)
    //    {
    //        Destroy(parent.GetChild(i).gameObject);
    //    }
    //}

    public void DisplayNextActivity()
    {
        DisplayActivity(currentNoteIndex + 1);
    }

    public void DisplayPreviousActivity()
    {
        DisplayActivity(currentNoteIndex - 1);
    }
}
