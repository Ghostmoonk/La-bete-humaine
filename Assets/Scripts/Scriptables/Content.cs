using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum ContentType
{
    OpenQuestion, ClosedQuestion, FillGaps, SimpleText, Button
}


public abstract class Content
{
    public UnityEvent CompleteEvent = new UnityEvent();
    public UnityEvent AdditionalEvents = new UnityEvent();

    List<Observer> observers = new List<Observer>();

    public void Complete()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete();
        }
        AdditionalEvents?.Invoke();
    }

    public void Complete(Content content)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete(content);
        }
        AdditionalEvents?.Invoke();
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
    public UnityEvent CompleteEvent;
}