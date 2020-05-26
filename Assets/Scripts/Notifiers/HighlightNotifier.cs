using System.Collections;
using System.Collections.Generic;
using TextContent;
using UnityEngine;

public class HighlightNotifier : Subject
{
    public new List<GlossaryObserver> observers = new List<GlossaryObserver>();

    public override void Complete()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnComplete();
        }
    }

    public override void AddObserver(Observer observer)
    {
        observers.Add((GlossaryObserver)observer);
    }

    public void BroadcastHighlight(Vector3 pos, GlossaryData data = null)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnHighligh(pos, data);
        }
    }
}
