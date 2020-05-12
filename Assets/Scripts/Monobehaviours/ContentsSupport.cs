using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


//Gère l'affichage des textes sur l'HUD

public class ContentsSupport : MonoBehaviour
{
    #region Components
    Animator animator;
    #endregion

    #region GameObjectsReferences
    Transform tabletContentContainer;
    GameObject scrollDownArrow;
    #endregion

    GameObject currentGameObjectToShow;

    public List<ContentRef> contentsType;
    private Queue<Content> contents;
    ContentDisplayer contentDisplayer;
    bool active;

    #region Prefabs
    [SerializeField] GameObject simpleTextPref;
    [SerializeField] GameObject openQuestionPref;
    [SerializeField] GameObject closedQuestionPreb;
    [SerializeField] GameObject fillGapsPref;
    #endregion

    #region Inputs
    Vector2 previousMousePos = Vector2.zero;
    Vector2 currentMousePos = Vector2.zero;
    #endregion
    public event EventHandler<EventArgs> TabletActiveEvent;

    private void OnEnable()
    {
        active = false;
    }

    public bool IsOnScreen()
    {
        return active;
    }

    private void Update()
    {
        #region Inputs

        #region Scroll
        if (scrollDownArrow.activeSelf)
        {
            previousMousePos = Input.mousePosition;
            Invoke(nameof(SetCurrentMousePosition), Time.deltaTime);
        }
        else if (previousMousePos != Vector2.zero)
        {
            previousMousePos = Vector2.zero;
        }
        else if (currentMousePos != Vector2.zero)
        {
            currentMousePos = Vector2.zero;
        }
        #endregion

        if (Input.GetButtonDown("Jump"))
        {
            DisplayContent();
        }

        #endregion
    }
    void SetCurrentMousePosition()
    {
        currentMousePos = Input.mousePosition;
    }
    public void ToggleVisibility()
    {
        animator.SetBool("Active", !animator.GetBool("Active"));
        active = animator.GetBool("Active");
    }

    private void Start()
    {
        contentDisplayer = new ContentDisplayer();
        contentDisplayer.disPlayNextContentDelegateFunction = DisplayContent;
        animator = GetComponent<Animator>();
        scrollDownArrow = GameObject.FindGameObjectWithTag("ScrollDownArrow");
        scrollDownArrow.SetActive(false);
        tabletContentContainer = GameObject.FindGameObjectWithTag("TabletContent").transform;

        contents = InitializeContents();

        Debug.Log("Il y a " + contents.Count + "contents");
        DisplayContent();

    }

    public Queue<Content> InitializeContents()
    {
        Queue<Content> contentsQueue = new Queue<Content>();
        for (int i = 0; i < contentsType.Count; i++)
        {
            contentsQueue.Enqueue(ContentFactory.CreateContent(contentsType[i].id, contentsType[i].type));

        }
        return contentsQueue;
    }


    private void DisplayContent()
    {
        if (contents.Count <= 0)
            return;

        GameObject contentToInstantiate;
        Content currentContent = contents.Dequeue();
        currentContent.AddObserver(contentDisplayer);
        if (currentContent.GetType() == typeof(SimpleText))
        {
            SimpleText st = (SimpleText)currentContent;
            simpleTextPref.SetActive(false);

            contentToInstantiate = Instantiate(simpleTextPref, tabletContentContainer);
            contentToInstantiate.GetComponent<TextHolder>().simpleText = st;
            Debug.Log(contentToInstantiate.GetComponent<TextHolder>().simpleText.textData.content);
            simpleTextPref.SetActive(true);

            //StartCoroutine(nameof(WaitLectureTime));
        }

        else if (currentContent.GetType() == typeof(OpenQuestion) || currentContent.GetType() == typeof(ClosedQuestion))
        {
            if (currentContent.GetType() == typeof(OpenQuestion))
            {
                openQuestionPref.SetActive(false);
                OpenQuestion oq = (OpenQuestion)currentContent;
                contentToInstantiate = Instantiate(openQuestionPref, tabletContentContainer);
                contentToInstantiate.GetComponent<OpenQuestionHolder>().openQuestion = oq;
                openQuestionPref.SetActive(true);
            }
            else
            {
                contentToInstantiate = Instantiate(closedQuestionPreb, tabletContentContainer);
            }
        }
        else
        {
            contentToInstantiate = Instantiate(fillGapsPref, tabletContentContainer);
        }

        contentToInstantiate.SetActive(true);
        currentGameObjectToShow = contentToInstantiate;
        Resizer.ResizeHeight(tabletContentContainer.GetComponent<RectTransform>());
    }

    private void DisplayScrollArrow()
    {
        scrollDownArrow.SetActive(true);
    }

    #region Anim events actions
    public void Scroll(BaseEventData eventData)
    {
        Debug.Log(previousMousePos - currentMousePos);
        if (scrollDownArrow.activeSelf && (Input.mouseScrollDelta.y < 1 || previousMousePos.y - currentMousePos.y > 0f))
        {
            scrollDownArrow.SetActive(false);
        }
    }

    public void InActivePlace()
    {
        TabletActiveEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

}