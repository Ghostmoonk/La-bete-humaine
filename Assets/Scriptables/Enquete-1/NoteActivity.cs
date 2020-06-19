using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class NoteActivity : ScriptableObject
{
    [Header("Contenu")]
    public Manuscript[] manuscripts;
    public string title;
    [TextArea(7, 12)]
    public string fullText;
    public string paratext;
    public List<ProvidedWord> providedWords;

    [Header("Prefab")]
    [SerializeField] protected GameObject exercicePrefab;
    [SerializeField] protected GameObject manuscriptPrefab;

    public virtual GameObject GetExercicePrefab() { return exercicePrefab; }
    public virtual GameObject GetManuscriptPrefab() { return manuscriptPrefab; }
}

[System.Serializable]
public struct Manuscript
{
    public Sprite sprite;
    public string paratext;
}

[System.Serializable]
public struct ProvidedWord
{
    public string word;
    public WordType type;
}

public enum WordType { Nom, Lieu, Action, Adjective }