using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManuscriptActivityHolder : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI paratext;
    Sprite sprite;

    private void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public virtual void SetContent(Sprite sprite, string paratext)
    {
        this.sprite = sprite;
        this.paratext.text = paratext;
    }

    public void SetSprite()
    {
        image.sprite = sprite;
    }
}

public interface IFocusable
{
    event Action OnFocusIn;
    event Action OnFocusOut;
    void FocusIn();
    void FocusOut();
}

