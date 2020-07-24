using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

//Handle the locomotive route : stations, anomalies and distances
public class LocomotiveRouteManager : MonoBehaviour
{
    private static LocomotiveRouteManager instance;
    public static LocomotiveRouteManager Instance
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
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
    }

    LocomotiveRouteManagerUI routeManagerUI;

    [SerializeField] MovementReferenceObject locomotive;

    [SerializeField] StationData stationDeparture;
    StationData currentStationSegment;
    [SerializeField] RouteEvents[] routeEvents;

    RouteEvents currentsRouteEvents;

    private float routeTotalDistance;
    private float routeDistanceUntilLastStation = 0f;

    private void Start()
    {
        UpdateCurrentRouteEvents();

        currentStationSegment = stationDeparture;
        UpdateCurrentRouteEvents();

        routeManagerUI = LocomotiveRouteManagerUI.Instance;
        routeManagerUI.SetupStationsOnFrieze(stationDeparture);

        routeTotalDistance = CalculateRouteDist();

    }

    private float CalculateRouteDist()
    {
        StationData currentStation = stationDeparture;
        float routeDist = 0f;
        while (currentStation != null)
        {
            routeDist += currentStation.distwithNextStation;
            currentStation = currentStation.nextStation;
        }

        return routeDist;
    }

    public StationData GetCurrentStationInRoute() => currentStationSegment;

    public float GetRouteTotalDistance() => routeTotalDistance;

    public float GetRouteTraveledRatio() => locomotive.distanceDone / routeTotalDistance;

    private void Update()
    {
        if (currentsRouteEvents != null)
            //For every events registered for the route
            for (int i = 0; i < currentsRouteEvents.routeEventArray.Count; i++)
            {
                //If the distance between the last station and the moving object is equal to the lerped distance of the last station and the next with the indicated value between 0 & 1
                if (locomotive.distanceDone - routeDistanceUntilLastStation >= Mathf.Lerp(0, currentsRouteEvents.stationSegment.distwithNextStation, currentsRouteEvents.routeEventArray[i].lerpedDistance))
                {
                    currentsRouteEvents.routeEventArray[i].whatShallHappen?.Invoke();
                    currentsRouteEvents.routeEventArray.RemoveAt(i);
                }
            }

        if (locomotive.distanceDone - routeDistanceUntilLastStation >= currentStationSegment.distwithNextStation)
        {
            currentStationSegment = currentStationSegment.nextStation;
            UpdateCurrentRouteEvents();
        }
    }

    private void UpdateCurrentRouteEvents()
    {
        //if (currentsRouteEvents.routeEventArray.Count > 0)
        //    currentsRouteEvents.routeEventArray.Clear();

        //foreach (RouteEvents routeE in routeEvents)
        //{
        //    if (currentStationSegment == routeE.stationSegment)
        //    {
        //        currentsRouteEvents.Add(routeE);
        //    }
        //}

        foreach (RouteEvents item in routeEvents)
        {
            if (currentStationSegment == item.stationSegment)
            {
                currentsRouteEvents = item;
            }
        }

        routeDistanceUntilLastStation = locomotive.distanceDone;
    }

    public void Debug_(StationData stationData)
    {
        Debug.Log("Distance parcourue depuis la station :" + stationData.stationName + ", arrivée à " + stationData.nextStation.stationName + " en " + locomotive.distanceDone);
    }
}

[System.Serializable]
public class RouteEvents
{
    //Where
    public StationData stationSegment;
    public List<RouteEvent> routeEventArray;

}

[System.Serializable]
public struct RouteEvent
{
    [Range(0, 1)]
    [Tooltip("Between 0 & 1 : 0 = departure station, 1 = next station or arrival")]
    public float lerpedDistance;
    //something happen
    public UnityEvent whatShallHappen;
}
