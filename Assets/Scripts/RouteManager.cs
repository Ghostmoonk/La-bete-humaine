using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RouteManager : MonoBehaviour
{
    private static RouteManager instance;
    public static RouteManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Tooltip("Les arrêts de l'itinéraire")]
    [SerializeField] ArretData[] arrets;

    RouteItem[] routeItems;

    [Header("Components")]
    [SerializeField] GameObject stationRowPrefab;
    [SerializeField] Transform stationRowParent;

    RouteUI routeUI;

    public event Action OnUIReady;
    [Header("Events")]
    [SerializeField] UnityEvent OnAllItemsFill;
    [SerializeField] UnityEvent OnAllItemsNotfill;
    public UnityEvent OnAllItemsSucceed;
    bool allItemsFill = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        routeUI = GetComponent<RouteUI>();
        StartCoroutine(InstantiateRoute());
        Resizer.ResizeLayout(GetComponent<RectTransform>());
    }

    public RouteItem[] GetRouteItems() => routeItems;

    private IEnumerator InstantiateRoute()
    {
        if (!stationRowPrefab.GetComponent<RouteItem>())
            throw new Exception("Le prefab ne contient pas de script RouteItem");

        routeItems = new RouteItem[arrets.Length];

        for (int i = 0; i < arrets.Length; i++)
        {
            GameObject instantiatedFields = Instantiate(stationRowPrefab, stationRowParent);
            RouteItem currentRouteItem = instantiatedFields.GetComponent<RouteItem>();
            currentRouteItem.SetArretData(arrets[i]);

            routeItems[i] = currentRouteItem;
        }

        routeUI.SetRouteNumber(arrets.Length);

        yield return new WaitForSeconds(Time.deltaTime * 10);

        OnUIReady.Invoke();
    }

    public void CheckFieldsFill()
    {
        //On regarde si tout est fill
        foreach (RouteItem item in routeItems)
        {
            if (item.fill)
                continue;
            else
            {
                if (allItemsFill)
                {
                    OnAllItemsNotfill?.Invoke();
                    allItemsFill = false;
                }
                return;
            }
        }
        allItemsFill = true;
        OnAllItemsFill?.Invoke();
    }

    public void VerifyFields()
    {
        Dictionary<RouteItem, bool> itemsDico = new Dictionary<RouteItem, bool>();

        foreach (RouteItem item in routeItems)
        {
            itemsDico.Add(item, item.CheckValidity());
        }

        StartCoroutine(routeUI.DisplayRouteItemVerificationUI(itemsDico));
    }
}

[System.Serializable]
public struct ArretData
{
    public string stationName;
    public string tunnelName;
    public Relief relief;

}

public enum Relief
{
    Descente, Rampe, Plateau
}