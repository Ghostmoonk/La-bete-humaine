using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MeteoData")]
public class MeteoData : ScriptableObject
{
    public string weatherTitle;
    [TextArea(3, 5)]
    public string weatherText;
    public string weatherParatext;

    public Relief newRelief;
    public Sprite newReliefIcon;
    public string meteoText;
}