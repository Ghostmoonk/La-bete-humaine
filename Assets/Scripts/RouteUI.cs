using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteUI : MonoBehaviour
{
    [SerializeField] RectTransform blueLine;
    [SerializeField] RectTransform stationsBloc;

    [SerializeField] TextMeshProUGUI stationAmountText;
    [SerializeField] Button verifyButton;

    [SerializeField] RouteManager routeManager;

    [Range(0.1f, 1f)]
    [SerializeField] float verifyIconWaterfallDisplayDelay;

    private void Start()
    {
        routeManager.OnUIReady += SetBlueLineHeight;
    }

    public void SetBlueLineHeight()
    {
        blueLine.sizeDelta = new Vector2(blueLine.sizeDelta.x, stationsBloc.sizeDelta.y + blueLine.transform.parent.GetComponent<RectTransform>().sizeDelta.y * 2);
    }

    public void SetRouteNumber(int amount)
    {
        stationAmountText.text = "Trajet : " + amount + " arrêts";
    }

    public void ToggleVerifyButtonInteractable(bool toggle)
    {
        verifyButton.interactable = toggle;
    }

    public IEnumerator DisplayRouteItemVerificationUI(Dictionary<RouteItem, bool> items)
    {
        bool allSucceed = true;
        foreach (KeyValuePair<RouteItem, bool> item in items)
        {
            if (item.Value)
            {
                item.Key.TriggerAnimator("Succeed");
                item.Key.LockFields();
            }
            else
            {
                item.Key.TriggerAnimator("Fail");
                if (allSucceed)
                    allSucceed = false;
            }
            yield return new WaitForSeconds(verifyIconWaterfallDisplayDelay);

        }
        if (allSucceed)
        {
            RouteManager.Instance.OnAllItemsSucceed?.Invoke();
        }
    }

    public void HideRouteItemVerificationUI()
    {
        foreach (RouteItem item in RouteManager.Instance.GetRouteItems())
        {
            item.TriggerAnimator("Hide");
        }
    }
}
