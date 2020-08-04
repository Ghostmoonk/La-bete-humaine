using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Locomotive Anomaly")]
public class LocomotiveAnomalyData : ScriptableObject
{
    public string title;
    [TextArea(2, 4)]
    public string questionText;
    public LocomotiveComponent locomotiveComponent;
    public ScriptableAnswer[] answers;
    public float charcoalCostFail;
    public float timeCostFail;
}

[System.Serializable]
public class ScriptableAnswer
{
    [TextArea(1, 3)]
    public string answerText;
    public bool isCorrect;
}

public enum LocomotiveComponent
{
    Aucun,
    PresseDeGraissage,
    Cendrier,
    Souffleur,
    VolantDeCommande,
    Sifflet,
    Sablière,
    Foyer,
    Injecteur,
    Manomètre,
    Régulateur
}
