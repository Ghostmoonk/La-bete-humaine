using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            popup.SetWordText(data.word);
            popup.SetDefinitionText(data.definition);
            popup.GetComponent<RectTransform>().anchoredPosition = pos;
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
