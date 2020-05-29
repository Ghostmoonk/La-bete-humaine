using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;
    public static DialogManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        dialogBox.SetActive(false);
    }

    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI sentenceText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image skipImage;

    Dialog currentDialog;

    public void StartDialog()
    {
        dialogBox.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentDialog.currentSentence != null)
        {
            characterNameText.text = currentDialog.currentSentence.characterName;
            sentenceText.text = currentDialog.currentSentence.content;
            //Prepare next sentence
            currentDialog.SetNextSentence();
        }
        else
        {
            StopDialog();
        }
    }

    public void StopDialog()
    {
        currentDialog = null;
        dialogBox.SetActive(false);
    }

    public void SetNewDialog(int dialogId)
    {
        currentDialog = DialogsLoader.Instance.dialogsDico[dialogId];
    }

}
