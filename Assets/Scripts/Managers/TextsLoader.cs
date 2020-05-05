using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextContent;
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

    public Dictionary<int, Text> textsDico;

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
        textsDico = new Dictionary<int, Text>();
        TextAsset textsData = Resources.Load<TextAsset>("Textes");

        string[] data = textsData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { '|' });

            Text textRow = new Text(row[1], row[2], row[3], row[4]);
            textsDico[int.Parse(row[0])] = textRow;

            //Debug.Log(textsDico[int.Parse(row[0])].content);
        }
    }
}
