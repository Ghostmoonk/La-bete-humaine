using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextModifier : MonoBehaviour
{
    private static TextModifier instance;
    public static TextModifier Instance
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


    public IEnumerator ProgressChangeText(TextMeshProUGUI textMesh, string newText, float changeTextSpeed)
    {
        while (textMesh.text.Length != 0)
        {
            textMesh.text = textMesh.text.Substring(0, textMesh.text.Length - 1);
            yield return new WaitForSeconds(changeTextSpeed * Time.deltaTime);
        }
        for (int i = 0; i < newText.Length; i++)
        {
            textMesh.text += newText[i];
            yield return new WaitForSeconds(changeTextSpeed * Time.deltaTime);
        }
        StopCoroutine(ProgressChangeText(textMesh, newText, changeTextSpeed));
    }

}
