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

    GlossaryData currentWordData;
    [SerializeField] Vector3 offset;

    private void OnEnable()
    {
        popup = FindObjectOfType<GlossaryPopUp>();
        popup.gameObject.SetActive(false);
    }
    private void Start()
    {
        glossaryObserver = new GlossaryObserver();
        glossaryObserver.glossaryDelegate += SetPopUpGlossary;
    }

    private void SetPopUpGlossary(Vector3 pos, GlossaryData data)
    {
        if (!popup.gameObject.activeSelf)
            popup.gameObject.SetActive(true);

        currentWordData = data;


        if (currentWordData != null)
        {
            RectTransform popupTransform = popup.GetComponent<RectTransform>();
            popup.SetWordText(data.word);
            popup.SetDefinitionText(data.definition);
            pos.z = popup.transform.position.z;

            Vector3 offsetPos;
            offsetPos = Camera.main.ScreenToWorldPoint(pos + offset);
            popupTransform.position = offsetPos;

            Vector2 adjustingOffset = Vector2.zero;
            if (popupTransform.anchoredPosition.x + popupTransform.sizeDelta.x > popupTransform.parent.GetComponent<CanvasScaler>().referenceResolution.x)
            {
                adjustingOffset.x = -popupTransform.sizeDelta.x - (offset.x * 2);
            }
            if (popupTransform.anchoredPosition.y + popupTransform.sizeDelta.y > 0)
            {
                adjustingOffset.y = -popupTransform.sizeDelta.y - (offset.y * 2);
            }
            popupTransform.anchoredPosition += adjustingOffset;
        }
        else
        {
            currentWordData = null;
            HidePopUp();
        }
    }

    private void HidePopUp()
    {
        popup.gameObject.SetActive(false);
    }

}
