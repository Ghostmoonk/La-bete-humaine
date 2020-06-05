using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum ContentType
{
    OpenQuestion, ClosedQuestion, FillGaps, SimpleText, SimpleImage
}

public abstract class Subject
{
    protected List<Observer> observers = new List<Observer>();

    public abstract void Complete();

    public virtual void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public virtual void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

public abstract class Content : Subject
{
    public UnityEvent CompleteEvent = new UnityEvent();
    //public UnityEvent AdditionalEvents = new UnityEvent();


    public override void Complete()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete();
        }
        //AdditionalEvents?.Invoke();
    }

}

public class SliderSubject : Subject
{

    public override void Complete()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete();
        }
    }
    public void Complete(ReferenceImage RefImg)
    {
        Debug.Log(observers.Count);
        for (int i = 0; i < observers.Count; i++)
        {
            if (observers[i].GetType() == typeof(SliderObserver))
            {
                SliderObserver obs = (SliderObserver)observers[i];
                obs.OnSlide(RefImg);
            }
        }
    }

}

[Serializable]
public struct ContentRef
{
    public ContentType type;
    public int id;
    public UnityEvent CompleteEvent;
    [HideInInspector] public UnityEvent OnSelectEvent;
}