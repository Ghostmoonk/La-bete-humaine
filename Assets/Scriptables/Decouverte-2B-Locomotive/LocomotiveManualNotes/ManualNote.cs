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
    [TextArea(2, 3)]
    public string textParatext;
    public Sprite manuscrit;
    [TextArea(2, 3)]
    public string manuscritParatext;

}

public enum AnomalieType
{
    Pression, Graissage, Visibilité, Terrain, Vitesse, Enlisement, Lison
}
