using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnomalyManager : MonoBehaviour
{
    private static AnomalyManager instance;
    public static AnomalyManager Instance
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

    LocomotiveAnomalyData currentAnomaly;
    [SerializeField] AnomalyManagerUI anomalyManagerUI;
    [SerializeField] UnityEventLocomotiveData OnVerifyCorrect;
    [SerializeField] UnityEventLocomotiveData OnVerifyFailed;

    [SerializeField] AnomalyEventsSerialized[] anomaliesEventsArray;
    public Dictionary<LocomotiveAnomalyData, AnomalyEvents> anomaliesEventsDico { get; private set; }

    public AnomalyEvents GetAnomalyEvents(LocomotiveAnomalyData locoData) => anomaliesEventsDico[locoData];

    private void Start()
    {
        anomaliesEventsDico = new Dictionary<LocomotiveAnomalyData, AnomalyEvents>();
        foreach (AnomalyEventsSerialized item in anomaliesEventsArray)
        {
            anomaliesEventsDico.Add(item.locoAnoData, item.anomalyEvents);
        }
    }

    public LocomotiveAnomalyData GetCurrentAnomaly() => currentAnomaly;

    public void TriggerAnomaly(LocomotiveAnomalyData anomalyData)
    {
        currentAnomaly = anomalyData;
        anomalyManagerUI.SetAnomalyContent(currentAnomaly);
    }

    public void VerifyAnswers(Dictionary<ScriptableAnswer, bool> answersDico)
    {
        bool succeed = true;
        Dictionary<ScriptableAnswer, bool> correctionDico = new Dictionary<ScriptableAnswer, bool>();

        for (int i = 0; i < currentAnomaly.answers.Length; i++)
        {
            if (!answersDico.ContainsKey(currentAnomaly.answers[i]))
                throw new Exception("On a reçu une réponse ne faisant pas partie de l'actuelle anomalie ??");

            //if the player has one uncorrect answer
            if (answersDico[currentAnomaly.answers[i]] != currentAnomaly.answers[i].isCorrect)
            {
                succeed = false;
            }

            correctionDico.Add(currentAnomaly.answers[i], answersDico[currentAnomaly.answers[i]] == currentAnomaly.answers[i].isCorrect);
        }

        if (succeed)
        {
            OnVerifyCorrect?.Invoke(currentAnomaly);
        }
        else
        {
            Debug.Log("fail !");
            OnVerifyFailed?.Invoke(currentAnomaly);
        }
        anomalyManagerUI.ShowVerification(correctionDico, succeed);
    }

}

[Serializable]
public struct AnomalyEvents
{
    public UnityEvent OnResolveFail;
    public UnityEvent OnResolveSuccess;
}

[Serializable]
public struct AnomalyEventsSerialized
{
    public LocomotiveAnomalyData locoAnoData;
    public AnomalyEvents anomalyEvents;
}