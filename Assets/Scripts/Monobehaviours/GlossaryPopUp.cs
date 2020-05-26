using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlossaryPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wordTextMesh;
    [SerializeField] TextMeshProUGUI definitionTextMesh;

    public void SetWordText(string word)
    {
        wordTextMesh.text = word;
    }

    public void SetDefinitionText(string def)
    {
        definitionTextMesh.text = def;
    }
}
