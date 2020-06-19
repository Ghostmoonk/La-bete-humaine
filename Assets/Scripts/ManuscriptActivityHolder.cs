using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManuscriptActivityHolder : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI paratext;

    private void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public virtual void SetContent(Sprite sprite, string paratext)
    {
        image.sprite = sprite;
        this.paratext.text = paratext;
    }
}
