using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FillGapsActivityHolder : ActivityHolder
{
    //Input field prefab
    [SerializeField] GameObject inputFieldPrefab;
    //List of associated prefab object that will be instantiated
    List<TextGap> textGaps;
    //To keep track on the selected input field
    int currentSelectedIndex = -1;

    //Observer of all TextGap subject
    FillGapsActivityObserver observer;

    //Fire when all gaps are complete and succeed
    public UnityEvent OnGapsComplete;

    protected new FillGapsNoteActivity noteActivity;

    public override void SetContent(NoteActivityEvent noteActivity)
    {
        this.noteActivity = (FillGapsNoteActivity)noteActivity.noteActivity;

        contentTextMesh.text = this.noteActivity.gapsText;
        paratextTextMesh.text = this.noteActivity.paratext;

        GeneralCompleteEvent = noteActivity.GeneralCompleteEvents;
    }

    public override void CompleteActivity()
    {
        base.CompleteActivity();

    }

    public override void ProvideWords()
    {
        notesDisplayer.wordSorter.RevealWords(noteActivity.providedWords);
    }

    protected override void Start()
    {
        base.Start();
        textGaps = new List<TextGap>();
        observer = new FillGapsActivityObserver();

        observer.fillGapsDelegate += CheckAllGapsSucceed;
        observer.gapSelectedDelegate += SetCurrentGapSelected;

        Invoke(nameof(SetUpInputFields), Time.deltaTime * 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetCurrentGapSelected(currentSelectedIndex + 1);
            textGaps[currentSelectedIndex].inputFieldMesh.Select();
            Debug.Log(currentSelectedIndex);
        }
    }

    public void SetCurrentGapSelected(int index)
    {
        if (index >= textGaps.Count)
            currentSelectedIndex = 0;
        else
            currentSelectedIndex = index;
    }

    public void SetCurrentGapSelected(TextGap textGap)
    {
        if (textGaps.IndexOf(textGap) == -1)
            throw new System.Exception("Un input field du texte à toru n'est pas référencé dans son activité");

        currentSelectedIndex = textGaps.IndexOf(textGap);
        Debug.Log("selected :" + currentSelectedIndex);
    }

    private void SetUpInputFields()
    {
        for (int i = 0; i < contentTextMesh.textInfo.characterCount; i++)
        {
            if (contentTextMesh.textInfo.characterInfo[i].character == '_')
            {
                int startIndex = i;
                int cursor = startIndex;
                int spaceAmount = 0;

                while (contentTextMesh.textInfo.characterInfo[cursor].character == '_')
                {
                    cursor++;
                    spaceAmount++;
                    ChangeCharacterColor(cursor - 1, new Color(contentTextMesh.color.r, contentTextMesh.color.g, contentTextMesh.color.b, 0f));
                }

                Vector3 pos = contentTextMesh.transform.TransformPoint(contentTextMesh.textInfo.characterInfo[startIndex + (spaceAmount / 2)].bottomLeft);
                GameObject textInput = Instantiate(inputFieldPrefab, pos, Quaternion.identity, contentTextMesh.transform);
                string hiddenWord = noteActivity.fullText.Substring(startIndex, spaceAmount);

                textInput.GetComponent<TextGap>().Setup(hiddenWord);
                textInput.GetComponent<TextGap>().textGapSubject.AddObserver(observer);
                //textInput.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, textInput.GetComponent<RectTransform>().sizeDelta.y / 2);

                textGaps.Add(textInput.GetComponent<TextGap>());

                i += spaceAmount;
            }
        }
    }

    private bool CheckAllGapsSucceed()
    {
        OnGapsComplete?.Invoke();

        for (int i = 0; i < textGaps.Count; i++)
        {
            if (textGaps[i].succeed == false)
                return false;
        }
        CompleteActivity();
        return true;
    }

    public void BoldifyGapsText()
    {
        for (int i = 0; i < textGaps.Count; i++)
        {
            textGaps[i].inputFieldMesh.text = "<b>" + textGaps[i].inputFieldMesh.text + "</b>";
            Debug.Log(textGaps[i].inputFieldMesh.text);
        }
    }

    public void DisableInputFields()
    {
        for (int i = 0; i < textGaps.Count; i++)
        {
            textGaps[i].inputFieldMesh.interactable = false;
            textGaps[i].inputFieldMesh.image.color = Color.clear;
        }
    }

    public void ChangeWordColor(TMP_WordInfo wordInfo, Color32 color)
    {
        for (int i = 0; i < wordInfo.characterCount; ++i)
        {
            int charIndex = wordInfo.firstCharacterIndex + i;
            int meshIndex = contentTextMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
            int vertexIndex = contentTextMesh.textInfo.characterInfo[charIndex].vertexIndex;

            Color32[] vertexColors = contentTextMesh.textInfo.meshInfo[meshIndex].colors32;

            //We loop 4 times because tehre are 4 corners on the character vertice
            for (int j = 0; j < 4; j++)
            {
                vertexColors[vertexIndex + j] = color;
            }
        }
        contentTextMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    public void ChangeCharacterColor(int charIndex, Color32 color)
    {

        int meshIndex = contentTextMesh.textInfo.characterInfo[charIndex].materialReferenceIndex;
        int vertexIndex = contentTextMesh.textInfo.characterInfo[charIndex].vertexIndex;

        Color32[] vertexColors = contentTextMesh.textInfo.meshInfo[meshIndex].colors32;

        //We loop 4 times because tehre are 4 corners on the character vertice
        for (int j = 0; j < 4; j++)
        {
            vertexColors[vertexIndex + j] = color;
        }
        contentTextMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
