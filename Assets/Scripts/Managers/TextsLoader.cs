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
        FetchGlossaryData();

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public Dictionary<int, TextData> textsDico;
    public Dictionary<int, QuestionData> questionsDico;
    public Dictionary<int, GlossaryData> glossaryDico;

    private void FetchGlossaryData()
    {
        glossaryDico = new Dictionary<int, GlossaryData>();
        TextAsset textsData = Resources.Load<TextAsset>("Glossary");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });
            GlossaryData glossaryData;

            if (row[3].Contains(".png") || row[3].Contains(".jpg") || row[3].Contains(".jpeg"))
            {
                glossaryData = new GlossaryData(row[1], row[2], row[3]);
            }
            else
            {
                glossaryData = new GlossaryData(row[1], row[2]);
            }
            glossaryDico[int.Parse(row[0])] = glossaryData;
        }
    }

    private void FetchTextsData()
    {
        textsDico = new Dictionary<int, TextData>();
        TextAsset textsData = Resources.Load<TextAsset>("TextesTest");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });
            string text = row[3].Replace("\\n", "\n");
            int minReadTime = 0;
            int.TryParse(row[7], out minReadTime);
            Debug.Log("paratext length :" + row[9].Length);
            Debug.Log("title length :" + row[2].Length);
            TextData textRow = new TextData(row[2], text, row[9], row[4], row[5], row[6], minReadTime, row[8]);
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

            string[] justifTextIDstring = row[3].Split(new char[] { ',' });
            //Si question à choix multiples
            if (answers.Length > 1)
            {
                Dictionary<int, string> answerRefDico = new Dictionary<int, string>();
                //Debug.Log("Nombre d'ensemble de reponse" + justifTextIDstring.Length);

                for (int j = 0; j < answers.Length; j++)
                {
                    string[] textsRefIdsString = justifTextIDstring[j].Split(new char[] { '-' });

                    for (int l = 0; l < textsRefIdsString.Length; l++)
                    {
                        answerRefDico.Add(int.Parse(textsRefIdsString[l]), answers[j]);
                    }
                }

                //foreach (var item in answerRefDico)
                //{
                //    Debug.Log(item.Key + " - " + item.Value);
                //}
                ClosedQuestionData questionRow = new ClosedQuestionData(row[1], answerRefDico);
                questionsDico[int.Parse(row[0])] = questionRow;

            }
            //Sinon on passe juste le nom de la question
            else
            {
                OpenQuestionData questionRow = new OpenQuestionData(row[1], row[4]);
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
        Sprite manuscritSprite = Resources.Load<Sprite>("images/" + textsDico[id].manuscritPath.Split('.')[0]);

        return manuscritSprite;
    }

    public int ContainWordInGlossary(string word)
    {
        foreach (var item in glossaryDico)
        {
            if (item.Value.word.ToLower().Normalize() == word.ToLower().Normalize())
            {
                return item.Key;
            }
        }
        return -1;
    }
}
