using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RouteItem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_InputField stationNameInput;
    [SerializeField] TMP_Dropdown tunnelInput;
    [SerializeField] TMP_Dropdown stationReliefInput;

    [SerializeField] Animator verifyIconAnimator;

    ArretData arretData;

    public bool succeed { get; private set; }
    public bool fill { get; private set; }

    [SerializeField] UnityEvent OnStationNameLengthIncrease;
    [SerializeField] UnityEvent OnStationNameLengthDecrease;
    string currentStationName = "";

    public Animator GetVerifyIconAnimator() => verifyIconAnimator;

    public void SetArretData(ArretData data) =>
        arretData = data;

    public bool CheckValidity()
    {
        if (stationNameInput.text.ToLower() == arretData.stationName.ToLower()
            && tunnelInput.options[tunnelInput.value].text.ToLower() == arretData.tunnelName.ToLower()
            && stationReliefInput.options[stationReliefInput.value].text.ToLower() == arretData.relief.ToString().ToLower())
            return true;
        else
            return false;

    }

    public void LockFields()
    {
        stationNameInput.interactable = false;
        tunnelInput.interactable = false;
        stationReliefInput.interactable = false;
    }

    public void TriggerAnimator(string triggerName)
    {
        verifyIconAnimator.SetTrigger(triggerName);
    }

    public void CheckFill(string text)
    {
        //Si les 3 textes sont remplis
        if (stationNameInput.text.Length > 0 && tunnelInput.options[tunnelInput.value].text.Length > 0 && stationReliefInput.options[stationReliefInput.value].text.Length > 0)
        {
            fill = true;
        }
        else
            fill = false;

        RouteManager.Instance.CheckFieldsFill();
    }

    public void StationNameDiff(string newStationName)
    {
        Debug.Log(newStationName.Length);
        Debug.Log(currentStationName.Length);
        if (newStationName.Length > currentStationName.Length)
        {
            OnStationNameLengthIncrease?.Invoke();
        }
        else if (newStationName.Length < currentStationName.Length)
        {
            OnStationNameLengthDecrease?.Invoke();
        }

        currentStationName = newStationName;
    }

}
