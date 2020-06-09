using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHolder : MonoBehaviour
{
    [SerializeField] UnityEvent[] events;

    public void InvokeEvent(int index)
    {
        events[index].Invoke();
    }
}
