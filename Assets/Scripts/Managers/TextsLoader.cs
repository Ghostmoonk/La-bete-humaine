using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextContent;
using System.Linq;

//Charge et stocke les données récupérés dans le fichier CSV

public class TextsLoader : MonoBehaviour
{
    #region Singleton
    private static TextsLoader _instance;
    public static TextsLoader Instance
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

        FetchTextsData();
        FetchQuestionsData();
    }
    #endregion

    public Dictionary<int, TextData> textsDico;
    public Dictionary<int, QuestionData> questionsDico;
    //Pour associer un id à un texte

    private void FetchTextsData()
    {
        textsDico = new Dictionary<int, TextData>();
        TextAsset textsData = Resources.Load<TextAsset>("TextesTest");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });

            int minReadTime = 0;

            int.TryParse(row[6], out minReadTime);
            TextData textRow = new TextData(row[1], row[2], row[3], row[4], row[5], minReadTime, row[7]);
            textsDico[int.Parse(row[0])] = textRow;

        }
    }

    private void FetchQuestionsData()
    {
        questionsDico = new Dictionary<int, QuestionData>();
        TextAsset textsData = Resources.Load<TextAsset>("Questions");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });

            string[] answers = row[2].Split(new char[] { ',' });

            //Si question à choix multiples
            if (answers.Length > 1)
            {
                Dictionary<int, string> answerRefDico = new Dictionary<int, string>();
                string[] justifTextIDstring = row[3].Split(new char[] { ',' });
                int[] justifTextIDs = new int[justifTextIDstring.Length];

                for (int j = 0; j < justifTextIDs.Length; j++)
                {
                    justifTextIDs[j] = int.Parse(justifTextIDstring[j]);
                }
                for (int j = 0; j < answers.Length; j++)
                {
                    answerRefDico.Add(justifTextIDs[j], answers[j]);
                }

                QuestionData questionRow = new QuestionData(row[1], answerRefDico);
                questionsDico[int.Parse(row[0])] = questionRow;

            }
            //Sinon on passe juste le nom de la question
            else
            {
                QuestionData questionRow = new QuestionData(row[1]);
                questionsDico[int.Parse(row[0])] = questionRow;

            }
        }
    }


    public bool IsAssociatedWithManuscript(int id)
    {
        if (textsDico[id].manuscritPath != null)
            return true;
        else
            return false;

    }

    public Sprite FetchManuscriptSprite(int id)
    {
        Sprite manuscritSprite = Resources.Load<Sprite>("manuscrits/" + textsDico[id].manuscritPath.Split('.')[0]);

        return manuscritSprite;
    }
}
