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
    [SerializeField] RouteEvent[] routeEvents;

    List<RouteEvent> currentsRouteEvents;

    private float routeTotalDistance;
    private float routeDistanceUntilLastStation = 0f;

    private void Start()
    {
        currentsRouteEvents = new List<RouteEvent>();
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

    public float GetRouteTotalDistance() => routeTotalDistance;

    private void Update()
    {
        //For every events registered for the route
        for (int i = 0; i < currentsRouteEvents.Count; i++)
        {
            //If the distance between the last station and the moving object is equal to the lerped distance of the last station and the next with the indicated value between 0 & 1
            if (locomotive.distanceDone - routeDistanceUntilLastStation >= Mathf.Lerp(0, currentsRouteEvents[i].stationSegment.distwithNextStation, currentsRouteEvents[i].lerpedDistance))
            {
                currentsRouteEvents[i].whatShallHappen?.Invoke();
            }
        }
    }

    private void UpdateCurrentRouteEvents()
    {
        foreach (RouteEvent routeE in routeEvents)
        {
            if (currentStationSegment == routeE.stationSegment)
            {
                currentsRouteEvents.Add(routeE);
            }
        }
        routeDistanceUntilLastStation = locomotive.distanceDone;
    }
}

[System.Serializable]
public struct RouteEvent
{
    //Where
    public StationData stationSegment;
    [Range(0, 1)]
    [Tooltip("Between 0 & 1 : 0 = departure station, 1 = next station or arrival")]
    public float lerpedDistance;
    //something happen
    public UnityEvent whatShallHappen;

}
