using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputsManager : MonoBehaviour
{
    private static PlayerInputsManager instance;
    public static PlayerInputsManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Events")]
    public UnityEvent EscapePressedEvents;

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

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            EscapePressedEvents?.Invoke();
        }
    }
}
