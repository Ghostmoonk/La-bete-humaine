using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    private static EventsManager instance;
    public static EventsManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public UnityEvent StartSceneEvents;

    private void Start()
    {
        StartSceneEvents?.Invoke();
    }
}
