using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Station")]
public class StationData : ScriptableObject
{
    public string stationName;
    public StationData nextStation;
    [Tooltip("In meters")]
    public float distwithNextStation;
    public float altitude;
}
