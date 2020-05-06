using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


//Gère l'affichage des textes sur l'HUD

public class Tablet : MonoBehaviour
{
    #region Components
    Animator animator;
    #endregion

    #region GameObjectsReferences
    [SerializeField] GameObject textPrefab;
    Transform tabletContentContainer;
    GameObject scrollDownArrow;
    #endregion

    GameObject currentGameObjectToShow;

    public List<Content> contents;
    bool active;

    public event EventHandler<EventArgs> TabletActiveEvent;

    private void OnEnable()
    {
        active = false;
    }

    public void ToggleVisibility()
    {
        animator.SetBool("Active", !animator.GetBool("Active"));
        active = animator.GetBool("Active");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        scrollDownArrow = GameObject.FindGameObjectWithTag("ScrollDownArrow");
        scrollDownArrow.SetActive(false);
        tabletContentContainer = GameObject.FindGameObjectWithTag("TabletContent").transform;
        DisplayText(FetchText(0));
    }

    private void DisplayText(string text)
    {
        currentGameObjectToShow.GetComponent<TextMeshProUGUI>().text = text;
    }

    public string FetchText(int id)
    {
        //En allant chercher un nouveau texte, on a donc besoin d'un nouveau GameObject pour le contenir
        GameObject textToInstantiate = Instantiate(textPrefab, tabletContentContainer);
        TextMeshProUGUI textMesh = textToInstantiate.GetComponent<TextMeshProUGUI>();

        TextHolder textHolder = textToInstantiate.GetComponent<TextHolder>();
        textHolder.textData = TextsLoader.Instance.textsDico[id];

        currentGameObjectToShow = textToInstantiate;

        StartCoroutine(nameof(WaitBeforeScrollPossible));
        return TextsLoader.Instance.textsDico[id].content;
    }

    IEnumerator WaitBeforeScrollPossible()
    {
        float timer = 0f;

        while (timer < currentGameObjectToShow.GetComponent<TextHolder>().textData.minimumReadTime)
        {
            if (active)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        DisplayScrollArrow();
    }
    private void DisplayScrollArrow()
    {
        scrollDownArrow.SetActive(true);
    }

    #region Anim events actions
    public void Scroll(BaseEventData eventData)
    {
        Debug.Log("Scroll");
    }

    public void InActivePlace()
    {
        TabletActiveEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

}

//var builder = new System.Text.StringBuilder(text.Length * 26);
//Color32 color = contentText.color;