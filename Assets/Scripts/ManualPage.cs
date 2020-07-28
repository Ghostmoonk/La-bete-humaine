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
    [SerializeField] TextMeshProUGUI textParatext;
    [SerializeField] TextMeshProUGUI manuscriptParatext;
    [SerializeField] Image manuscritImage;

    public void SetupContents(AnomalieType anomalieType, string subTitle, string text, string paratext, Sprite manuscrit = null, string manuscriptText = null)
    {
        mainTitleText.text = anomalieType.ToString();
        subTitleText.text = subTitle;

        if (text.Length > 0)
        {
            textContent.text = text;
            textParatext.text = paratext;
        }
        else
        {
            textContent.gameObject.SetActive(false);
            textParatext.gameObject.SetActive(false);
        }

        if (manuscrit == null)
        {
            manuscritImage.gameObject.SetActive(false);
            manuscriptParatext.gameObject.SetActive(false);
        }
        else
        {
            manuscritImage.sprite = manuscrit;

            if (manuscriptText != null)
                manuscriptParatext.text = manuscriptText;
            else
                manuscriptParatext.gameObject.SetActive(false);

        }
    }
}
