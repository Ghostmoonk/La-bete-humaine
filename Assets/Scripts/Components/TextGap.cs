using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextGap : MonoBehaviour
{
    public TMP_InputField inputFieldMesh;
    RectTransform rectTransform;
    [SerializeField] LayoutElement layoutElt;
    [SerializeField] float characterWidth;

    [Header("Colors")]
    [SerializeField] Color defaultColor;
    [SerializeField] Color successColor;
    [SerializeField] Color failColor;
    string hiddenWord;

    [HideInInspector] public TextGapSubject textGapSubject;

    [HideInInspector] public bool succeed = false;

    private void Start()
    {
        defaultColor = inputFieldMesh.image.color;
    }

    public void Setup(string hiddenWord)
    {
        textGapSubject = new TextGapSubject();

        this.hiddenWord = hiddenWord;
        inputFieldMesh.characterLimit = hiddenWord.Length;
        inputFieldMesh.text = "";
        rectTransform = GetComponent<RectTransform>();
        layoutElt.minWidth = characterWidth * hiddenWord.Length;
        //rectTransform.sizeDelta = new Vector2(characterWidth * hiddenWord.Length, rectTransform.sizeDelta.y);

    }

    public void CheckWord(string currentText)
    {
        if (currentText.Length == hiddenWord.Length)
            if (currentText == hiddenWord)
            {
                inputFieldMesh.image.color = successColor;
                succeed = true;
                textGapSubject.Complete();
            }
            else
            {
                inputFieldMesh.image.color = failColor;
                succeed = false;
            }
        else
            inputFieldMesh.image.color = defaultColor;
    }

    public void Selected()
    {
        textGapSubject.Selected(this);
    }
}
