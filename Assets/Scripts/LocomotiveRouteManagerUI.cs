using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocomotiveRouteManagerUI : MonoBehaviour
{
    private static LocomotiveRouteManagerUI instance;
    public static LocomotiveRouteManagerUI Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Meteo Info UI")]
    [SerializeField] Image topologyImage;
    [SerializeField] TextMeshProUGUI topologyText;
    [SerializeField] Image weatherImage;
    [SerializeField] TextMeshProUGUI weatherText;

    [Header("Route info")]
    [SerializeField] LineRenderer frieze;
    [SerializeField] GameObject stationIconPrefab;
    [SerializeField] TextMeshProUGUI hoveredStationText;

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

    public void SetupStationsOnFrieze(StationData firstStation)
    {
        StationData currentStationData = firstStation;
        int compteur = 0;

        while (currentStationData != null)
        {
            GameObject iconStationToInstantiate = Instantiate(stationIconPrefab, frieze.transform);
            iconStationToInstantiate.gameObject.name = "IconStation - " + currentStationData.stationName;
            iconStationToInstantiate.GetComponent<RectTransform>().anchoredPosition = frieze.GetPosition(compteur);
            SetUpStationIcon(iconStationToInstantiate, currentStationData);

            compteur++;
            currentStationData = currentStationData.nextStation;
        }
    }

    private void SetUpStationIcon(GameObject iconObject, StationData stationData)
    {
        //iconObject.GetComponent<Button>().onClick.AddListener(() => ShowHoveredStation(stationData.stationName));

        EventTrigger trigger = iconObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryExit.eventID = EventTriggerType.PointerExit;

        entryEnter.callback.AddListener(delegate { Debug.Log("hover"); ShowHoveredStation(stationData.stationName); });
        entryExit.callback.AddListener(delegate { HideHoveredStation(); });
        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    private void ShowHoveredStation(string stationName)
    {
        hoveredStationText.gameObject.SetActive(true);
        hoveredStationText.text = stationName;
    }
    private void HideHoveredStation() => hoveredStationText.gameObject.SetActive(false);

}
