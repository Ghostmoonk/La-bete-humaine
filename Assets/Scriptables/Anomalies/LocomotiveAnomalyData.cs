using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Locomotive Anomalie")]
public class LocomotiveAnomalyData : ScriptableObject
{
    /*An anomaly is made of :
    * What happens
    * Where it happens on the board
    * A question to answer
    * Answers
    */
    public string title;
    [TextArea(2, 4)]
    public string questionText;
    public LocomotiveComponent locomotiveComponent;
    public ScriptableAnswer[] answers;

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
    PresseGraissage,
    Cendrier,
    Souffleur,
    VolantCommande,
    Sifflet,
    Sablière,
    Foyer,
    Injecteur,
    Manomètre,
    Régulateur
}
