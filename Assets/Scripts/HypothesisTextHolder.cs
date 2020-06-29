using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HypothesisTextHolder : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [SerializeField] Image icon;
    [SerializeField] Image cross;
    public UnityEvent RevealEvent;

    public void SetIconSprite(Sprite newSprite)
    {
        icon.sprite = newSprite;
    }

    public void Cross()
    {
        cross.gameObject.SetActive(true);
    }
}
