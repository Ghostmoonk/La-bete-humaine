using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Content currentContent;

    public List<ContentRef> contentsType;
    private Stack<Content> contentsStack;
    public ContentDisplayer contentDisplayer;
    bool active;

    #region Prefabs
    [SerializeField] GameObject simpleTextPref;
    [SerializeField] GameObject titledTextPref;
    [SerializeField] GameObject openQuestionPref;
    [SerializeField] GameObject closedQuestionPreb;
    [SerializeField] GameObject fillGapsPref;
    [SerializeField] GameObject EndButton;
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

        if (Input.GetKeyDown(KeyCode.Dollar))
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

        contentsStack = new Stack<Content>();
        contentDisplayer = new ContentDisplayer();
        contentDisplayer.displayNextContentDelegateFunction = DisplayNextContent;
        animator = GetComponent<Animator>();
        scrollDownArrow = GameObject.FindGameObjectWithTag("ScrollDownArrow");
        scrollDownArrow.SetActive(false);
        tabletContentContainer = GameObject.FindGameObjectWithTag("TabletContent").transform;

        contentsStack = InitializeContents();

        Debug.Log("Il y a " + contentsStack.Count + "contents");
        DisplayContent();

    }

    public Stack<Content> InitializeContents()
    {
        List<Content> contentsList = new List<Content>();
        Stack<Content> contentsStack = new Stack<Content>();
        for (int i = 0; i < contentsType.Count; i++)
        {
            Content content = ContentFactory.CreateContent(contentsType[i].id, contentsType[i].type);
            content.CompleteEvent = contentsType[i].CompleteEvent;
            contentsList.Add(content);
        }
        Debug.Log(contentsList.Count);
        for (int i = 0; i < contentsList.Count; i++)
        {
            contentsStack.Push(contentsList[contentsList.Count - i - 1]);
        }
        contentsStack.Reverse();
        return contentsStack;
    }

    public void AddContentInStack(Content content)
    {
        contentsStack.Push(content);
    }

    private void DisplayNextContent()
    {
        DisplayContent(null);
    }

    public Content GetCurrentContent()
    {
        return currentContent;
    }

    private void DisplayContent(Content content = null)
    {
        if (contentsStack.Count <= 0 && content == null)
        {
            //S'il n'y a plus de contenu à afficher et qu'on cherche à en afficher, c'est la fin de la scène
            //Instantiate(EndButton, tabletContentContainer);
            return;
        }
        else
        {
            GameObject contentToInstantiate;
            if (content == null)
                currentContent = contentsStack.Pop();
            else
                currentContent = content;

            //Debug.Log(currentContent);
            currentContent.AddObserver(contentDisplayer);

            if (currentContent.GetType() == typeof(SimpleText))
            {
                SimpleText st = (SimpleText)currentContent;
                //Sans un titre
                if (st.textData.title == "")
                {
                    simpleTextPref.SetActive(false);

                    contentToInstantiate = Instantiate(simpleTextPref, tabletContentContainer);
                    contentToInstantiate.GetComponent<TextHolder>().simpleText = st;
                    simpleTextPref.SetActive(true);
                }
                //Avec un titre
                else
                {
                    titledTextPref.SetActive(false);

                    contentToInstantiate = Instantiate(titledTextPref, tabletContentContainer);
                    contentToInstantiate.GetComponent<TextHolder>().simpleText = st;
                    Debug.Log(contentToInstantiate.GetComponent<TextHolder>().simpleText.textData.content);
                    titledTextPref.SetActive(true);
                }

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
                    closedQuestionPreb.SetActive(false);
                    ClosedQuestion cq = (ClosedQuestion)currentContent;
                    contentToInstantiate = Instantiate(closedQuestionPreb, tabletContentContainer);
                    contentToInstantiate.GetComponent<ClosedQuestionHolder>().closedQuestion = cq;
                    closedQuestionPreb.SetActive(true);
                }
            }
            else
            {
                contentToInstantiate = Instantiate(fillGapsPref, tabletContentContainer);
            }

            contentToInstantiate.SetActive(true);
            currentGameObjectToShow = contentToInstantiate;
        }
        Resizer.ResizeHeight(tabletContentContainer.GetComponent<RectTransform>());

        if (tabletContentContainer.parent.parent.GetComponent<RectTransform>().sizeDelta.y < tabletContentContainer.GetComponent<RectTransform>().sizeDelta.y)
            DisplayScrollArrow();

    }

    private void DisplayScrollArrow()
    {
        scrollDownArrow.SetActive(true);
    }

    public void CreateFinalButton(string text)
    {
        GameObject specialOption = Instantiate(EndButton, tabletContentContainer);
        specialOption.GetComponentInChildren<TextMeshProUGUI>().text = text;
        //return specialOption;
    }

    #region Anim events actions
    public void Scroll(BaseEventData eventData)
    {
        //Debug.Log(previousMousePos - currentMousePos);
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