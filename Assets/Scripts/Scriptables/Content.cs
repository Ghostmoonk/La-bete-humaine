using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ContentType
{
    OpenQuestion, ClosedQuestion, FillGaps, SimpleText
}

public abstract class Content
{
    protected string name;
    protected GameObject prefab;

    List<Observer> observers = new List<Observer>();

    public void Notify()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify();
        }
    }

    public void Complete()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete();
        }
    }


    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

[Serializable]
public struct ContentRef
{
    public ContentType type;
    public int id;
}