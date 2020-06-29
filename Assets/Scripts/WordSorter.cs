using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Instantiate prefabs of words given in a NotesDisplayer depending on their type
//Handle the hypothesis words
public class WordSorter : MonoBehaviour
{
    [SerializeField] NotesDisplayer notesDisplayer;
    [SerializeField] GameObject wordPrefab;

    [Header("Words containers")]
    [SerializeField] WordTypeContainer[] wordsContainers;
    Dictionary<WordType, Transform> wordsDicoContainers;
    Dictionary<ProvidedWord, GameObject> wordsObjectsContainers;

    private void Start()
    {
        foreach (RectTransform item in GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        }
    }

    public void InitializeWords()
    {
        wordsDicoContainers = new Dictionary<WordType, Transform>();
        wordsObjectsContainers = new Dictionary<ProvidedWord, GameObject>();

        for (int i = 0; i < wordsContainers.Length; i++)
        {
            wordsDicoContainers.Add(wordsContainers[i].wordType, wordsContainers[i].container);
        }

        foreach (NoteActivity item in notesDisplayer.GetNotes())
        {
            List<ProvidedWord> providedWords = new List<ProvidedWord>();
            for (int i = 0; i < item.providedWords.Count; i++)
            {
                if (!providedWords.Contains(item.providedWords[i]))
                {
                    providedWords.Add(item.providedWords[i]);
                }
            }
            InstantiateWords(providedWords);
        }
    }

    private void InstantiateWords(List<ProvidedWord> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            if (!wordsObjectsContainers.ContainsKey(words[i]))
            {
                GameObject wordToInstantiate = Instantiate(wordPrefab, wordsDicoContainers[words[i].type]);
                wordToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = words[i].word;
                wordsObjectsContainers.Add(words[i], wordToInstantiate);
            }
        }
    }

    public void InstantiateWords(NoteActivity noteActivity)
    {
        for (int i = 0; i < noteActivity.providedWords.Count; i++)
        {
            if (!wordsObjectsContainers.ContainsKey(noteActivity.providedWords[i]))
            {
                GameObject wordToInstantiate = Instantiate(wordPrefab, wordsDicoContainers[noteActivity.providedWords[i].type]);
                wordToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = noteActivity.providedWords[i].word;
                wordsObjectsContainers.Add(noteActivity.providedWords[i], wordToInstantiate);
            }
        }
    }

    public void RevealWords(List<ProvidedWord> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            if (wordsObjectsContainers.ContainsKey(words[i]))
            {
                wordsObjectsContainers[words[i]].GetComponentInChildren<GraphicFader>().FadeOut(1);
            }
        }
    }

    public void RevealWords(NoteActivity noteActivity)
    {
        for (int i = 0; i < noteActivity.providedWords.Count; i++)
        {
            if (wordsObjectsContainers.ContainsKey(noteActivity.providedWords[i]))
            {
                wordsObjectsContainers[noteActivity.providedWords[i]].GetComponentInChildren<GraphicFader>().FadeOut(1);
            }
        }
    }
}

[Serializable]
public struct WordTypeContainer
{
    public Transform container;
    public WordType wordType;
}