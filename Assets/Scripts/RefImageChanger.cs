using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefImageChanger : MonoBehaviour
{
    public SliderObserver observer = new SliderObserver();
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI reference;
    [SerializeField] TextMeshProUGUI story;
    [SerializeField] Image icon;
    [SerializeField] Image iconBack;

    [SerializeField] Sprite toggleOnSprite;
    [SerializeField] Sprite toggleOffSprite;

    private void Start()
    {
        observer.sliderDelegate += UpdateTexts;
    }
    private void UpdateTexts(ReferenceImage refImg)
    {
        Debug.Log("oui");
        title.text = refImg.title;
        reference.text = refImg.reference;

        if (refImg.story != "")
        {
            if (!story.gameObject.activeSelf)
                story.gameObject.SetActive(true);

            story.text = refImg.story;
        }
        else
            story.gameObject.SetActive(false);
    }

    public void ToggleSprite()
    {
        if (icon.sprite == toggleOffSprite)
        {
            icon.sprite = toggleOnSprite;
            iconBack.sprite = toggleOnSprite;
        }
        else
        {
            icon.sprite = toggleOffSprite;
            iconBack.sprite = toggleOffSprite;
        }
    }
}
