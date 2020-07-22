using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manual Note")]
public class ManualNote : ScriptableObject
{
    public AnomalieType anomalieType;
    public string title;

    [TextArea(4, 8)]
    public string text;
    public Sprite manuscrit;

}

public enum AnomalieType
{
    Pression, Graissage, Météo, Terrain
}
