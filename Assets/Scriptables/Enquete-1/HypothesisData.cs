using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hypothese")]
public class HypothesisData : ScriptableObject
{
    public int relatedDialogID;
    [TextArea(3, 5)]
    public string text;
    public string condensedText;
}
