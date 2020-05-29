using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Charge tous les dialogues de la scène et les stocke dans un dictionnaire <int,Dialog>

public class DialogsLoader : MonoBehaviour
{
    public TextAsset sentencesAsset;
    public TextAsset dialogsAsset;

    private static DialogsLoader _instance;
    public static DialogsLoader Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        FetchDialogsData();

    }

    public Dictionary<int, Dialog> dialogsDico;
    public UnityEvent DialogsLoadComplete;

    private void FetchDialogsData()
    {
        dialogsDico = new Dictionary<int, Dialog>();

        string[] dialogsRows = dialogsAsset.text.Split(new char[] { '\n' });
        string[] sentencesRows = sentencesAsset.text.Split(new char[] { '\n' });

        //Loop over all dialogs
        for (int i = 1; i < dialogsRows.Length - 1; i++)
        {
            string[] dialogCols = dialogsRows[i].Split(new char[] { '|' });

            Dictionary<int, Sentence> sentences = new Dictionary<int, Sentence>();
            //Loop over all sentences
            for (int j = 1; j < sentencesRows.Length - 1; j++)
            {
                string[] sentenceCols = sentencesRows[j].Split(new char[] { '|' });
                //If the sentence's dialog ID match the Dialog ID 
                if (int.Parse(dialogCols[0]) == int.Parse(sentenceCols[4]))
                {
                    sentences.Add(int.Parse(sentenceCols[0]), new Sentence(sentenceCols[1], sentenceCols[2]));
                }

            }
            int rootId = 0; ;
            for (int j = 1; j < sentencesRows.Length - 1; j++)
            {
                string[] sentenceCols = sentencesRows[j].Split(new char[] { '|' });
                if (sentences.ContainsKey(int.Parse(sentenceCols[0])))
                {
                    int previousSentenceID;
                    if (int.TryParse(sentenceCols[5], out previousSentenceID))
                    {
                        sentences[previousSentenceID].SetNextSentence(sentences[int.Parse(sentenceCols[0])]);
                    }
                    else
                    {
                        rootId = int.Parse(sentenceCols[0]);
                    }
                }
            }
            //One loop for dialog over
            dialogsDico.Add(int.Parse(dialogCols[0]), new Dialog(sentences[rootId]));
        }
        Debug.Log("complete");
        DialogsLoadComplete?.Invoke();
    }
}
