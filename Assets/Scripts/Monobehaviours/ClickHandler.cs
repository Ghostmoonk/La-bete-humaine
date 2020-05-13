using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{
    [HideInInspector]
    public static int remainToClick;

    Content currentContent;
    Content[] contentToDisplay;
    Transform relatedContentTransform;
    [HideInInspector] public Animator animator;

    public void SetContents(Transform relatedObject, Content currentContent, int[] idsContentsToDisplay = null)
    {
        this.currentContent = currentContent;
        this.relatedContentTransform = relatedObject;

        if (idsContentsToDisplay != null)
        {
            contentToDisplay = new Content[idsContentsToDisplay.Length];

            for (int i = 0; i < contentToDisplay.Length; i++)
            {
                contentToDisplay[i] = ContentFactory.CreateContent(idsContentsToDisplay[i], ContentType.SimpleText);

                if (i == contentToDisplay.Length - 1)
                {
                    contentToDisplay[i].AdditionalEvents.AddListener(relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFadeIn);
                }
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        FindObjectOfType<ContentsSupport>().contentDisplayer.displayNextContentDelegateFunction += ReplaceContentAtBottom;
    }
    public void Click()
    {
        relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFade(false);
        //S'il ne reste plus qu'un choix quand on click
        if (remainToClick < 2)
        {
            FindObjectOfType<ContentsSupport>().contentDisplayer.displayNextContentDelegateFunction -= ReplaceContentAtBottom;

        }

        GetComponent<Button>().interactable = false;
        remainToClick--;
        for (int i = 0; i < contentToDisplay.Length; i++)
        {
            FindObjectOfType<ContentsSupport>().AddContentInStack(contentToDisplay[i]);
            Debug.Log(contentToDisplay[i]);
            if (i == contentToDisplay.Length - 1)
            {
                contentToDisplay[i].AdditionalEvents.AddListener(relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFadeIn);
                if (remainToClick == 0)
                    contentToDisplay[i].AdditionalEvents.AddListener(currentContent.CompleteEvent.Invoke);
                //FindObjectOfType<ContentsSupport>().GetCurrentContent().AdditionalEvents.AddListener(relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFadeIn);
            }
        }

    }

    public void ReplaceContentAtBottom()
    {
        relatedContentTransform.SetAsLastSibling();
    }

}
