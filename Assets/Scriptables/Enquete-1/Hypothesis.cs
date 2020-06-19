using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hypothese")]
public class Hypothesis : ScriptableObject
{
    [TextArea(3, 5)]
    public string text;
}
