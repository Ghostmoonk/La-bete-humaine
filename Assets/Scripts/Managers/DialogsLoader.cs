using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//Charge tous les dialogues de la scène et les stocke dans un dictionnaire <int,Dialog>

public class DialogsLoader : MonoBehaviour
{
    public TextAsset sentencesAsset;
    public TextAsset dialogsAsset;
    public TextAsset answersAsset;

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

        if (answersAsset != null)
            FetchAnswersData();

        FetchDialogsData();

    }

    public Dictionary<int, Dialog> dialogsDico;
    //Key : sentence related ID, Value : answer text
    public Dictionary<int, string> answersSentenceIdDico;
    public UnityEvent DialogsLoadComplete;

    private void FetchAnswersData()
    {
        answersSentenceIdDico = new Dictionary<int, string>();

        string[] answersRows = answersAsset.text.Split(new char[] { '\n' });

        for (int i = 1; i < answersRows.Length - 1; i++)
        {
            string[] answersCols = answersRows[i].Split(new char[] { '|' });

            answersSentenceIdDico.Add(int.Parse(answersCols[1]), answersCols[3]);
        }
    }

    private void FetchDialogsData()
    {
        dialogsDico = new Dictionary<int, Dialog>();

        string[] dialogsRows = dialogsAsset.text.Split(new char[] { '\n' });
        string[] sentencesRows = sentencesAsset.text.Split(new char[] { '\n' });
        Debug.Log(dialogsRows.Length);
        //Loop over all dialogs
        for (int i = 1; i < dialogsRows.Length - 1; i++)
        {
            Debug.Log(i);
            string[] dialogCols = dialogsRows[i].Split(new char[] { '|' });
            Debug.Log(dialogsRows[i]);
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

            int rootId = 0;

            for (int j = 1; j < sentencesRows.Length - 1; j++)
            {
                string[] sentenceCols = sentencesRows[j].Split(new char[] { '|' });

                if (sentences.ContainsKey(int.Parse(sentenceCols[0])))
                {
                    //Does the sentence need answers
                    if (sentenceCols[6].Length >= 1)
                    {
                        if (answersSentenceIdDico == null)
                        {
                            throw new Exception("L'asset des dialogues requiert des réponses, et aucun asset de réponses n'a été donné.");
                        }

                        string[] splitAnswersID = sentenceCols[6].Split(',');
                        Answer[] answers = new Answer[splitAnswersID.Length];

                        for (int l = 0; l < splitAnswersID.Length; l++)
                        {
                            answers[l] = new Answer(answersSentenceIdDico[int.Parse(splitAnswersID[l])], sentences[int.Parse(splitAnswersID[l])]);
                        }

                        sentences[int.Parse(sentenceCols[0])] = new QuestioningSentence(sentenceCols[1], sentenceCols[2], answers);
                    }

                    int previousSentenceID;

                    if (int.TryParse(sentenceCols[5], out previousSentenceID))
                    {
                        Debug.Log(sentences[previousSentenceID]);
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
        DialogsLoadComplete?.Invoke();

    }
}
