using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ManuscriptToggler : MonoBehaviour, IPointerDownHandler
{
    Sprite spriteRef;
    Vector2 spriteSize;
    GameObject manuscriptImage;
    GameObject manuscriptContainer;
    TextMeshProUGUI manuscriptSource;

    RectTransform supportTransform;

    bool toggle;

    private void Start()
    {
        if (GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath != null)
        {
            toggle = false;
            spriteRef = Resources.Load<Sprite>("manuscrits/" + GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath.Split('.')[0]);
            manuscriptSource = GameObject.FindGameObjectWithTag("ManuscriptSource").GetComponent<TextMeshProUGUI>();
            manuscriptContainer = GameObject.FindGameObjectWithTag("ManuscriptContainer");
            manuscriptImage = GameObject.FindGameObjectWithTag("ManuscriptContainer").transform.GetChild(0).gameObject;
            spriteSize = spriteRef.rect.size;
            supportTransform = FindObjectOfType<ContentsSupport>().GetComponent<RectTransform>();
        }
        else
            gameObject.SetActive(false);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toggle = !toggle;

        //Cache
        if (!toggle)
        {
            manuscriptContainer.GetComponent<RectTransform>().DOAnchorPosX(supportTransform.anchorMin.x + manuscriptContainer.GetComponent<RectTransform>().sizeDelta.x / 2, 2f);
        }
        //Affiche
        else
        {
            manuscriptImage.GetComponent<Image>().sprite = spriteRef;
            manuscriptSource.text = GetComponentInParent<TextHolder>().simpleText.textData.manuscritSource;

            manuscriptImage.GetComponent<RectTransform>().sizeDelta = spriteSize;
            //Aligne l'image verticalement à la flèche
            manuscriptContainer.transform.position = new Vector3(manuscriptContainer.transform.position.x, transform.position.y, manuscriptContainer.transform.position.z);
            Debug.Log(supportTransform.anchorMin.x + manuscriptContainer.GetComponent<VerticalLayoutGroup>().padding.right - manuscriptContainer.GetComponent<RectTransform>().sizeDelta.x / 2);
            Debug.Log(manuscriptContainer.GetComponent<RectTransform>().sizeDelta.x / 2);
            manuscriptContainer.GetComponent<RectTransform>().anchoredPosition = new Vector3(supportTransform.anchorMin.x + spriteSize.x / 2, manuscriptContainer.GetComponent<RectTransform>().anchoredPosition.y);

            manuscriptContainer.GetComponent<RectTransform>().DOAnchorPosX(supportTransform.anchorMin.x - manuscriptImage.GetComponent<RectTransform>().sizeDelta.x / 2, 2f);

        }
    }

}
