using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReferenceImage")]
public class ReferenceImage : ScriptableObject
{
    public Sprite sprite;
    public string title;
    public string reference;

    [TextArea(3, 6)]
    public string story;

}
