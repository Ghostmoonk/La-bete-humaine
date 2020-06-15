using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//At beginning, instantiate all notes
//Manage their display
public class NotesDisplayer : MonoBehaviour
{
    [Header("Support")]
    [SerializeField] Transform exerciceContainer;
    [SerializeField] Transform rightPage;

    [Header("Navigation")]
    [SerializeField] Button previousNoteButton;
    [SerializeField] Button nextNoteButton;

    //[Header("Activity Prefabs")]
    //[SerializeField] GameObject readingPrefab;
    //[SerializeField] GameObject fillGapsPrefab;

    [SerializeField] TextMeshProUGUI titleMesh;
    [SerializeField] List<NoteActivity> notesActivities;
    [HideInInspector] public NoteActivity currentNote;

    private List<ActivityHolder> activityHolders;

    private int currentNoteIndex = 0;

    private void Start()
    {
        activityHolders = new List<ActivityHolder>();

        foreach (NoteActivity note in notesActivities)
        {
            InstantiateActivity(note);
        }

        DisplayActivity(0);
    }

    private void InstantiateActivity(NoteActivity note)
    {
        GameObject noteToInstantiate = Instantiate(note.GetPrefab(), exerciceContainer);

        if (noteToInstantiate.GetComponent<ActivityHolder>())
        {
            activityHolders.Add(noteToInstantiate.GetComponent<ActivityHolder>());
            noteToInstantiate.GetComponent<ActivityHolder>().SetContent(note);

            noteToInstantiate.SetActive(false);
        }
        else
            throw new Exception("Le prefab à instancier :" + currentNote.GetPrefab().name + ", n'a pas de composant ActivityHolder");

    }

    public void DisplayActivity(int index)
    {

        activityHolders[currentNoteIndex].gameObject.SetActive(false);
        if (index < notesActivities.Count && index >= 0)
        {
            currentNoteIndex = index;
            currentNote = notesActivities[currentNoteIndex];

            titleMesh.text = "Note n°" + (index + 1) + " : " + currentNote.title;

            activityHolders[currentNoteIndex].gameObject.SetActive(true);
            Resizer.ResizeHeight(activityHolders[currentNoteIndex].gameObject.GetComponent<RectTransform>());

            if (currentNoteIndex == notesActivities.Count - 1)
            {
                ToggleButtonInteractable(nextNoteButton, false);
                ToggleButtonInteractable(previousNoteButton, true);
            }
            else if (currentNoteIndex == 0)
            {
                ToggleButtonInteractable(nextNoteButton, true);
                ToggleButtonInteractable(previousNoteButton, false);
            }
            else
            {
                ToggleButtonInteractable(nextNoteButton, true);
                ToggleButtonInteractable(previousNoteButton, true);
            }

        }
        else if (index == -1)
        {
            //Afficher un sommaire
        }
    }

    private void ToggleButtonInteractable(Button button, bool interactable)
    {
        button.interactable = interactable;
    }

    private void RemoveChilds(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    public void DisplayNextActivity()
    {
        DisplayActivity(currentNoteIndex + 1);
    }

    public void DisplayPreviousActivity()
    {
        DisplayActivity(currentNoteIndex - 1);
    }


}
