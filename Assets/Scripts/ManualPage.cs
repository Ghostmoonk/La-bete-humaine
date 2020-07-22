using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManualPage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subTitleText;
    [SerializeField] TextMeshProUGUI mainTitleText;
    [SerializeField] TextMeshProUGUI textContent;
    [SerializeField] Image manuscritImage;

    public void SetupContents(AnomalieType anomalieType, string subTitle, string text, Sprite manuscrit = null)
    {
        mainTitleText.text = anomalieType.ToString();
        subTitleText.text = subTitle;
        textContent.text = text;

        if (manuscrit == null)
            manuscritImage.gameObject.SetActive(false);
        else
            manuscritImage.sprite = manuscrit;

    }
}
