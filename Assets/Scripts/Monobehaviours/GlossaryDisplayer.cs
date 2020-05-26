using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlossaryDisplayer : MonoBehaviour
{
    #region Singleton
    private static GlossaryDisplayer _instance;
    public static GlossaryDisplayer Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    GlossaryPopUp popup;
    public GlossaryObserver glossaryObserver;
    [SerializeField] ImageSlider imageSlider;

    GlossaryData currentWordData;
    //[SerializeField] Vector3 offset;

    private void OnEnable()
    {
        popup = FindObjectOfType<GlossaryPopUp>();
        popup.gameObject.SetActive(false);

    }
    private void Start()
    {
        glossaryObserver = new GlossaryObserver();
        glossaryObserver.glossaryDelegate += SetGlossary;
    }

    private void SetGlossary(Vector3 pos, GlossaryData data)
    {
        //Debug.Log(imageSlider.transform.TransformPoint(pos));
        if (data != null)
        {
            //We want to display an image
            if (data.imagePath != null)
            {
                imageSlider.SlideIn();
                Sprite spriteRef = Resources.Load<Sprite>("images/" + data.imagePath.Split('.')[0]);
                imageSlider.ShowImage(spriteRef, imageSlider.transform.TransformPoint(pos).y, data.definition);

                if (currentWordData != null)
                    if (currentWordData.imagePath == null)
                        HidePopUp();
            }
            //We want to display a definition
            else
            {
                if (!popup.gameObject.activeSelf)
                {
                    popup.gameObject.SetActive(true);
                }
                //If there is curently displayed a glossary image
                if (currentWordData != null)
                    if (currentWordData.imagePath != null)
                        imageSlider.SlideIn();

                popup.SetWordText(data.word);
                popup.SetDefinitionText(data.definition);
            }
            currentWordData = data;
        }
        else
        {
            if (currentWordData != null)
            {
                if (currentWordData.imagePath != null)
                    imageSlider.SlideIn();
                else
                    HidePopUp();
            }
            currentWordData = null;
        }
    }

    //Permet de placer le popup sur la souris avec un offset
    //private void PlaceOnPosition(Vector3 pos)
    //{
    //    RectTransform popupTransform = popup.GetComponent<RectTransform>();
    //    pos.z = popup.transform.position.z;

    //    Vector3 offsetPos;
    //    offsetPos = Camera.main.ScreenToWorldPoint(pos + offset);
    //    popupTransform.position = offsetPos;

    //    Vector2 adjustingOffset = Vector2.zero;
    //    if (popupTransform.anchoredPosition.x + popupTransform.sizeDelta.x > popupTransform.GetComponentInParent<CanvasScaler>().referenceResolution.x)
    //    {
    //        adjustingOffset.x = -popupTransform.sizeDelta.x - (offset.x * 2);
    //    }
    //    if (popupTransform.anchoredPosition.y + popupTransform.sizeDelta.y > 0)
    //    {
    //        adjustingOffset.y = -popupTransform.sizeDelta.y - (offset.y * 2);
    //    }
    //    popupTransform.anchoredPosition += adjustingOffset;
    //}

    private void HidePopUp()
    {
        popup.gameObject.SetActive(false);
    }

}
