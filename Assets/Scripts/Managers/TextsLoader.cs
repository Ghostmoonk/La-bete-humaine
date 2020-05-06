using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextContent;

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

    public Dictionary<int, TextData> textsDico;

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

        FetchData();
    }
    #endregion

    //Pour associer un id à un texte

    private void FetchData()
    {
        textsDico = new Dictionary<int, TextData>();
        TextAsset textsData = Resources.Load<TextAsset>("Textes");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });

            int minReadTime = 0;
            Debug.Log(row.Length);
            int.TryParse(row[6], out minReadTime);
            TextData textRow = new TextData(row[1], row[2], row[3], row[4], row[5], minReadTime, row[7]);
            textsDico[int.Parse(row[0])] = textRow;

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
