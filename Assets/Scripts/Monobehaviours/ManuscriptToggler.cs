using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManuscriptToggler : MonoBehaviour, IPointerDownHandler
{
    Sprite spriteRef;
    Vector2 spriteSize;
    GameObject manuscriptContainer;
    TextMeshProUGUI manuscriptSource;

    bool toggle;

    private void Start()
    {
        if (GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath != null)
        {
            toggle = false;
            spriteRef = Resources.Load<Sprite>("manuscrits/" + GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath.Split('.')[0]);
            manuscriptSource = GameObject.FindGameObjectWithTag("ManuscriptSource").GetComponent<TextMeshProUGUI>();
            manuscriptContainer = GameObject.FindGameObjectWithTag("ManuscriptContainer");
            spriteSize = spriteRef.rect.size;
        }
        else
            gameObject.SetActive(false);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toggle = !toggle;

        if (!toggle)
        {
            manuscriptContainer.GetComponent<RectTransform>().DOAnchorPosX(transform.parent.GetComponent<RectTransform>().anchoredPosition.x + transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2, 2f);
        }
        else
        {
            manuscriptContainer.GetComponent<Image>().sprite = spriteRef;
            manuscriptSource.text = GetComponentInParent<TextHolder>().simpleText.textData.manuscritSource;

            manuscriptContainer.GetComponent<RectTransform>().sizeDelta = spriteSize;
            manuscriptContainer.transform.position = new Vector3(manuscriptContainer.transform.position.x, transform.position.y, manuscriptContainer.transform.position.z);

            manuscriptContainer.GetComponent<RectTransform>().anchoredPosition = new Vector3(transform.parent.GetComponent<RectTransform>().anchoredPosition.x + transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2 + spriteSize.x / 2, manuscriptContainer.GetComponent<RectTransform>().anchoredPosition.y);
            manuscriptContainer.GetComponent<RectTransform>().DOAnchorPosX(transform.parent.GetComponent<RectTransform>().anchoredPosition.x + transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2 - spriteSize.x, 2f);
        }
    }
}
