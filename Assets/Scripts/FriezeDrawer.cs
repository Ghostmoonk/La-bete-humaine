using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriezeDrawer : MonoBehaviour
{
    [SerializeField] StationData initialStationData;
    [SerializeField] LineRenderer lineR;
    [SerializeField] Vector2 scale;
    [Min(1)]
    [SerializeField] int resolution;
    float currentMaxDistX;
    float maxXOnFrieze;

    [HideInInspector] public List<Vector3> stationsPositions = new List<Vector3>();

    public void DrawFrieze()
    {
        lineR.positionCount = 0;
        currentMaxDistX = 0f;
        StationData currentStation = initialStationData;

        lineR.positionCount = 1;
        stationsPositions.Clear();
        while (currentStation != null)
        {
            lineR.SetPosition((lineR.positionCount - 1), GetVertexPosition(currentStation));
            stationsPositions.Add(lineR.GetPosition((lineR.positionCount - 1)));
            currentStation = currentStation.nextStation;
            Debug.Log(lineR.GetPosition((lineR.positionCount - 1)));
            if (currentStation != null)
                lineR.positionCount += 1 * resolution;
        }
        Debug.Log(stationsPositions.Count);
        SmoothFrieze();
        Center();
    }

    public List<Vector3> GetStationsPositionsOnFrieze()
    {
        return stationsPositions;
    }

    private void SmoothFrieze()
    {
        //We get the number of stations : Ex : 7 stations, resolution : 4, 28 vertices. 28/4 = 7
        int initialLineVerticesAmount = lineR.positionCount / resolution;

        for (int j = 0; j < initialLineVerticesAmount; j++)
        {
            for (int i = 1; i < resolution; i++)
            {
                lineR.SetPosition(j * resolution + i,
                    Vector3.Lerp(
                        lineR.GetPosition(j * resolution),
                        lineR.GetPosition((j + 1) * resolution),
                       ((float)i / (float)(resolution))));
            }
        }
    }

    private void Center()
    {
        float xMax = float.MinValue;

        for (int i = 0; i < lineR.positionCount; i++)
        {
            if (lineR.GetPosition(i).x > xMax)
            {
                xMax = lineR.GetPosition(i).x;
            }
        }

        for (int i = 0; i < lineR.positionCount; i++)
        {
            lineR.SetPosition(i, new Vector3(lineR.GetPosition(i).x - xMax / 2, lineR.GetPosition(i).y, lineR.GetPosition(i).z));
            if (i % resolution == 0)
            {
                stationsPositions[i / resolution] =
                    new Vector3(stationsPositions[i / resolution].x - xMax / 2,
                    stationsPositions[i / resolution].y,
                    stationsPositions[i / resolution].z);
            }
        }
    }

    private Vector3 GetVertexPosition(StationData stationData)
    {
        Vector3 dist = new Vector3(currentMaxDistX * scale.x, stationData.altitude * scale.y, 0f); ;
        currentMaxDistX += stationData.distwithNextStation;
        return dist;
    }

}


