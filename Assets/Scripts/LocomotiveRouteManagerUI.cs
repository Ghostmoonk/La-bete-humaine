using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocomotiveRouteManagerUI : MonoBehaviour, IHaveTextChanging
{
    private static LocomotiveRouteManagerUI instance;
    public static LocomotiveRouteManagerUI Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Meteo Infos")]
    [SerializeField] Image topologyImage;
    [SerializeField] TextMeshProUGUI topologyText;
    [SerializeField] Image weatherImage;
    [SerializeField] TextMeshProUGUI weatherText;
    [SerializeField] RectTransform routeInfosParent;
    [SerializeField] RectTransform jaugesParent;

    [Header("Route infos")]
    [SerializeField] LineRenderer friezeLineR;
    [SerializeField] FriezeDrawer friezeDrawer;
    [SerializeField] GameObject stationIconPrefab;
    [SerializeField] TextMeshProUGUI hoveredStationText;
    [SerializeField] TextMeshProUGUI nextStationText;
    [SerializeField] float changeTextSpeed;
    Gradient friezeGradient;
    [Header("Gauges")]
    [SerializeField] Image charcoalGauge;
    [SerializeField] Image timeGauge;

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

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(routeInfosParent);
        LayoutRebuilder.ForceRebuildLayoutImmediate(jaugesParent);

        friezeGradient = new Gradient();
        friezeGradient.mode = GradientMode.Fixed;
    }

    public void UpdateNextStationText(StationData stationData)
    {
        ChangeText(nextStationText, stationData.nextStation.stationName, changeTextSpeed);
    }

    public void ChangeText(TextMeshProUGUI textMesh, string newText, float changeTextSpeed = 0)
    {
        StartCoroutine(TextModifier.Instance.ProgressChangeText(textMesh, newText, changeTextSpeed));
    }

    #region Frieze
    public void SetupStationsOnFrieze(StationData firstStation)
    {
        StationData currentStationData = firstStation;
        int compteur = 0;
        List<Vector3> stationsPosOnFrieze = friezeDrawer.GetStationsPositionsOnFrieze();

        while (currentStationData != null)
        {
            GameObject iconStationToInstantiate = Instantiate(stationIconPrefab, friezeLineR.transform);
            iconStationToInstantiate.gameObject.name = "IconStation - " + currentStationData.stationName;
            iconStationToInstantiate.GetComponent<RectTransform>().anchoredPosition = stationsPosOnFrieze[compteur];
            SetUpStationIcon(iconStationToInstantiate, currentStationData);

            compteur++;
            currentStationData = currentStationData.nextStation;
        }
    }

    private void Update()
    {
        #region FriezeGradientColor

        friezeGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(friezeLineR.startColor, Mathf.Clamp01(LocomotiveRouteManager.Instance.GetRouteTraveledRatio())),
                new GradientColorKey(friezeLineR.endColor, 1f) },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(friezeLineR.startColor.a, 0.0f),
                new GradientAlphaKey(friezeLineR.endColor.a, 1.0f)
            });
        friezeLineR.colorGradient = friezeGradient;

        //friezeImg.fillAmount = Mathf.Clamp01(LocomotiveRouteManager.Instance.GetRouteTraveledRatio());

        #endregion
    }

    private void SetUpStationIcon(GameObject iconObject, StationData stationData)
    {
        //iconObject.GetComponent<Button>().onClick.AddListener(() => ShowHoveredStation(stationData.stationName));

        EventTrigger trigger = iconObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryExit.eventID = EventTriggerType.PointerExit;

        entryEnter.callback.AddListener(delegate { ShowHoveredStation(stationData.stationName); });
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


    #endregion

}

public interface IHaveTextChanging
{
    void ChangeText(TextMeshProUGUI textMesh, string newText, float changeTextSpeed = 0f);
}
