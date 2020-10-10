using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Anecdote")]
public class AnecdoteData : ScriptableObject
{
    public AnecdoteContent[] anecdoteContents;
}

[Serializable]
public struct AnecdoteContent
{
    //public AnecdoteType type;
    [TextArea(3, 5)]
    public string title;
    [TextArea(3, 15)]
    public string text;
    public Sprite sprite;
}

public enum AnecdoteType
{
    Text, Image
}