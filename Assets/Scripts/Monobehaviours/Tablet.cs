using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


//Gère l'affichage des textes sur l'HUD

public class Tablet : MonoBehaviour
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
        animator = GetComponent<Animator>();
        scrollDownArrow = GameObject.FindGameObjectWithTag("ScrollDownArrow");
        scrollDownArrow.SetActive(false);
        tabletContentContainer = GameObject.FindGameObjectWithTag("TabletContent").transform;

        contents = InitializeContents();
        //DisplayContent(FetchContent(contents.Dequeue());

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
        GameObject contentToInstantiate;
        Content currentContent = contents.Dequeue();

        if (currentContent.GetType() == typeof(SimpleText))
        {
            SimpleText st = (SimpleText)currentContent;
            contentToInstantiate = Instantiate(simpleTextPref, tabletContentContainer);

            TextMeshProUGUI textMesh = contentToInstantiate.GetComponent<TextMeshProUGUI>();

            TextHolder textHolder = contentToInstantiate.GetComponent<TextHolder>();

            textHolder.textData = st.textData;
            textMesh.text = st.textData.content;
            currentGameObjectToShow = contentToInstantiate;

            StartCoroutine(nameof(WaitLectureTime));
        }

        if (currentContent.GetType() == typeof(OpenQuestion) || currentContent.GetType() == typeof(ClosedQuestion))
        {
            if (currentContent.GetType() == typeof(OpenQuestion))
            {
                OpenQuestion oq = (OpenQuestion)currentContent;
                contentToInstantiate = Instantiate(openQuestionPref, tabletContentContainer);

                TextMeshProUGUI question = contentToInstantiate.GetComponentInChildren<TextMeshProUGUI>();
                question.text = oq.questionData.question;
            }
            else
            {
                contentToInstantiate = Instantiate(closedQuestionPreb, tabletContentContainer);
                TextMeshProUGUI question = contentToInstantiate.GetComponentInChildren<TextMeshProUGUI>();
            }

        }
        Resizer.ResizeHeight(tabletContentContainer.GetComponent<RectTransform>());
    }

    IEnumerator WaitLectureTime()
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
        if (contents.Count > 0)
            DisplayContent();
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

//var builder = new System.Text.StringBuilder(text.Length * 26);
//Color32 color = contentText.color;