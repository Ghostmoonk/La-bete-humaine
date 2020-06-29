using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HypothesisManager : MonoBehaviour
{
    [SerializeField] HypothesisData[] hypothesesData;
    Hypothesis[] hypotheses;
    [SerializeField] HypothesisDisplayer displayer;
    int necessaryHypothesesCount;
    public event EventHandler<int> OnHypothesisDiscussed;
    private static HypothesisManager instance;
    public static HypothesisManager Instance
    {
        get
        {
            return instance;
        }
    }
    [SerializeField] int EndDialogID;
    public UnityEvent OnHypotheseSelectionOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        hypotheses = new Hypothesis[hypothesesData.Length];

        for (int i = 0; i < hypothesesData.Length; i++)
        {
            hypotheses[i] = new Hypothesis(hypothesesData[i]);

            if (hypothesesData[i].necessaryHypothesis)
                necessaryHypothesesCount++;
        }
    }

    private void Start()
    {
        OnHypothesisDiscussed += CrossHypothesis;
    }

    public void HypothesisDiscussed(int id)
    {
        OnHypothesisDiscussed?.Invoke(this, id);
    }

    private void CrossHypothesis(object sender, int id)
    {
        foreach (var item in hypotheses)
        {
            if (id == item.data.relatedDialogID)
                displayer.CrossHypothesis(item);
        }
    }

    public Hypothesis[] GetHypothesis()
    {
        return hypotheses;
    }

    public void CompleteHypothesis(int hypothesisIndex)
    {
        hypotheses[hypothesisIndex].found = true;

        displayer.RevealHypothesis(hypotheses[hypothesisIndex]);
    }

    public void SetHypothesisDiscussed(int hypothesisIndex)
    {
        hypotheses[hypothesisIndex].discussed = true;
    }

    public void InstantiateHypothesisAnswers(float appearDuration)
    {
        int foundedHypothesis = 0;

        for (int i = 0; i < hypotheses.Length; i++)
        {
            if (hypotheses[i].discussed)
            {
                foundedHypothesis++;
            }
            else if (hypotheses[i].found)
            {
                int n = i;
                GameObject answerToInstantiate = Instantiate(DialogManager.Instance.answerPrefab, DialogManager.Instance.answersContainer);
                answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = hypotheses[i].data.condensedText;
                answerToInstantiate.GetComponent<Button>().onClick.AddListener(delegate { DialogManager.Instance.SetNewDialog(hypotheses[n].data.relatedDialogID); DialogManager.Instance.StartDialog(); DialogManager.Instance.FadeClearAnswers(appearDuration); SetHypothesisDiscussed(n); });
                answerToInstantiate.GetComponent<GraphicFader>().FadeIn(appearDuration);

                foundedHypothesis++;
            }
        }

        if (foundedHypothesis < hypotheses.Length)
        {
            GameObject answerToInstantiate = Instantiate(DialogManager.Instance.answerPrefab, DialogManager.Instance.answersContainer);
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = "Non, je vais continuer de chercher.";

            answerToInstantiate.GetComponent<Button>().onClick.AddListener(delegate { DialogManager.Instance.ToggleDialogBoxVisibility(false); DialogManager.Instance.FadeClearAnswers(appearDuration); OnHypotheseSelectionOver.Invoke(); });
            answerToInstantiate.GetComponent<GraphicFader>().FadeIn(appearDuration);

        }

        int necessaryHypothesesFound = 0;
        for (int i = 0; i < hypotheses.Length; i++)
        {
            if (hypotheses[i].discussed && hypotheses[i].data.necessaryHypothesis)
                necessaryHypothesesFound++;
        }
        if (necessaryHypothesesFound >= necessaryHypothesesCount)
        {
            GameObject answerToInstantiate = Instantiate(DialogManager.Instance.answerPrefab, DialogManager.Instance.answersContainer);
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = "Non, je pense en avoir terminé.";

            answerToInstantiate.GetComponent<Button>().onClick.AddListener(delegate { DialogManager.Instance.SetNewDialog(EndDialogID); DialogManager.Instance.StartDialog(); ; DialogManager.Instance.FadeClearAnswers(appearDuration); });
            answerToInstantiate.GetComponent<GraphicFader>().FadeIn(appearDuration);
        }
        StartCoroutine(DialogManager.Instance.ResizeChilds(DialogManager.Instance.dialogBox.transform));
    }

}

public class Hypothesis
{
    public HypothesisData data;
    public bool found;
    public bool discussed;

    public Hypothesis(HypothesisData data)
    {
        this.data = data;
        found = false;
        discussed = false;
    }

}