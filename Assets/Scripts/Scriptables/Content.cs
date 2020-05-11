using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ContentType
{
    OpenQuestion, ClosedQuestion, FillGaps, SimpleText
}

public abstract class Content : MonoBehaviour
{
    protected string _name;
    protected GameObject prefab;

}

[Serializable]
public struct ContentRef
{
    public ContentType type;
    public int id;
}