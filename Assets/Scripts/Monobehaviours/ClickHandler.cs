using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                Debug.Log("Contenu à display : " + idsContentsToDisplay[i]);
                contentToDisplay[i] = ContentFactory.CreateContent(idsContentsToDisplay[i], ContentType.SimpleText);
                //À l'affichage du dernier contenu relatif à la réponse
                if (i == contentToDisplay.Length - 1)
                {
                    Debug.Log("Add listener");
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(contentToDisplay[i].CompleteEvent, relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFadeIn);
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(contentToDisplay[i].CompleteEvent, ReplaceContentAtBottom);
                    //contentToDisplay[i].CompleteEvent.AddListener(relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFadeIn);
                }
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Click()
    {
        relatedContentTransform.GetComponent<ClosedQuestionHolder>().StartProgressiveFade(false);
        //S'il ne reste plus qu'un choix quand on click
        //if (remainToClick < 2)
        //{
        //    FindObjectOfType<ContentsSupport>().contentDisplayer.displayNextContentDelegateFunction -= ReplaceContentAtBottom;

        //}

        GetComponent<Button>().interactable = false;
        remainToClick--;

        for (int i = 0; i < contentToDisplay.Length; i++)
        {
            FindObjectOfType<ContentsSupport>().AddContentInStack(contentToDisplay[contentToDisplay.Length - i - 1]);
        }

    }

    public void ReplaceContentAtBottom()
    {
        relatedContentTransform.SetAsLastSibling();
    }

}
