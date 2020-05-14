using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClosedQuestionHolder : MonoBehaviour
{
    [HideInInspector] public ClosedQuestion closedQuestion;

    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Transform optionsContainer;
    [SerializeField] GameObject optionPrefab;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        questionText.text = closedQuestion.questionData.question;

        StartCoroutine(InstantiateOptions());
    }

    private IEnumerator InstantiateOptions()
    {
        optionPrefab.SetActive(false);
        int optionsNumber = closedQuestion.questionData.answersRefDico.Values.Distinct().Count();
        ClickHandler.remainToClick = optionsNumber;

        List<GameObject> options = new List<GameObject>();
        foreach (var answer in closedQuestion.questionData.answersRefDico.Values.Distinct())
        {
            GameObject answerOption = Instantiate(optionPrefab, optionsContainer);
            answerOption.GetComponentInChildren<TextMeshProUGUI>().text = answer;
            answerOption.GetComponentInChildren<Button>().interactable = false;
            List<int> textsIdToDisplayList = new List<int>();
            foreach (var item in closedQuestion.questionData.answersRefDico)
            {
                if (item.Value == answer)
                {
                    //Debug.Log(item.Value + " - " + item.Key);
                    textsIdToDisplayList.Add(item.Key);
                }
            }
            int[] textsIdToDisplay = textsIdToDisplayList.ToArray();
            answerOption.GetComponent<ClickHandler>().SetContents(transform, closedQuestion, textsIdToDisplay);
            answerOption.SetActive(true);
            options.Add(answerOption);
            yield return new WaitForSeconds(0.3f);
        }

        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponentInChildren<Button>().interactable = true;
        }

        optionPrefab.SetActive(true);
        StopCoroutine(InstantiateOptions());
    }

    public void StartProgressiveFade(bool show)
    {
        StartCoroutine(ProgressiveFade(show));
    }

    public void StartProgressiveFadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(ProgressiveFade(true));
    }

    public IEnumerator ProgressiveFade(bool show)
    {
        //Si on a encore des boutons à afficher
        if (ClickHandler.remainToClick > 0)
        {
            //Récupère les boutons associés à cette question fermée
            gameObject.SetActive(true);
            ClickHandler[] buttons = GetComponentsInChildren<ClickHandler>();
            foreach (ClickHandler item in buttons)
            {
                item.animator.SetBool("Show", show);

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.2f);

            animator.SetBool("Show", show);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * animator.GetCurrentAnimatorStateInfo(0).speed);

            if (!show)
            {
                FindObjectOfType<ContentsSupport>().DisplayNextContent();
                StopCoroutine(ProgressiveFade(false));
                yield return null;
                gameObject.SetActive(false);

            }
            else
            {
                StartCoroutine(FindObjectOfType<ContentsSupport>().DisplayScrollArrow());
                StopCoroutine(ProgressiveFade(true));
            }
        }
        else
        {
            //Il n'y a plus de boutons à cliquer, on actualise le delegate de l'observateur
            if (closedQuestion.CompleteEvent.GetPersistentEventCount() > 0)
                FindObjectOfType<ContentsSupport>().contentDisplayer.onContentCompleteDelegate = closedQuestion.CompleteEvent.Invoke;

            closedQuestion.Complete();
            Debug.Log("closedQuestionComplete");
            gameObject.SetActive(false);
        }


    }

}
