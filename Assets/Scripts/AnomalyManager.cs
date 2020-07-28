using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] UnityEvent OnVerifyCorrect;
    [SerializeField] UnityEvent OnVerifyFailed;

    public void TriggerAnomaly(LocomotiveAnomalyData anomalyData)
    {
        currentAnomaly = anomalyData;
        anomalyManagerUI.SetAnomalyContent(currentAnomaly);
        anomalyManagerUI.ShowMarker(currentAnomaly);
    }

    public void VerifyAnswers(Dictionary<ScriptableAnswer, bool> answersDico)
    {
        bool succeed = true;

        for (int i = 0; i < currentAnomaly.answers.Length; i++)
        {
            if (!answersDico.ContainsKey(currentAnomaly.answers[i]))
                throw new Exception("On a reçu une réponse ne faisant pas partie de l'actuelle anomalie ??");

            if (!answersDico[currentAnomaly.answers[i]])
            {
                Debug.Log("On a pas eu bon.");
                succeed = false;
            }
        }

        if (succeed)
            OnVerifyCorrect?.Invoke();
        else
            OnVerifyFailed?.Invoke();
    }
}
