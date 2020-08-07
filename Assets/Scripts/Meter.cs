using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Do something after a certain distance reach
public class Meter : MonoBehaviour
{
    [SerializeField] PeriodicDistanceEvent[] periodicDistEvents;

    private void Update()
    {
        for (int i = 0; i < periodicDistEvents.Length; i++)
        {
            if (LocomotiveRouteManager.Instance.GetMovingObject().distanceDone - periodicDistEvents[i].compteur > periodicDistEvents[i].distance)
            {
                periodicDistEvents[i].OnDistanceReached?.Invoke();
                periodicDistEvents[i].compteur = LocomotiveRouteManager.Instance.GetMovingObject().distanceDone;
            }

        }
    }
}

[System.Serializable]
public struct PeriodicDistanceEvent
{
    public UnityEvent OnDistanceReached;
    public float distance;
    [HideInInspector] public float compteur;
}
