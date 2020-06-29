using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HypothesisDisplayer : MonoBehaviour
{
    [SerializeField] GameObject hypothesisPref;
    [SerializeField] Transform hypothesisContainer;
    [SerializeField] Sprite discoveredSprite;

    Dictionary<Hypothesis, HypothesisTextHolder> hypothesisDico;

    private void Start()
    {
        hypothesisDico = new Dictionary<Hypothesis, HypothesisTextHolder>();

        foreach (Hypothesis item in HypothesisManager.Instance.GetHypothesis())
        {
            GameObject hypotheseTextToInstantiate = Instantiate(hypothesisPref, hypothesisContainer);
            HypothesisTextHolder hypothesisTextHolder = hypotheseTextToInstantiate.GetComponent<HypothesisTextHolder>();

            hypothesisDico.Add(item, hypothesisTextHolder);
            hypothesisTextHolder.textComponent.text = item.data.text;
        }

        foreach (var item in GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.GetComponent<RectTransform>());
        }

    }

    public void CrossHypothesis(Hypothesis hypothesis)
    {
        hypothesisDico[hypothesis].Cross();
    }

    public void RevealHypothesis(Hypothesis hypothesisKey)
    {
        hypothesisDico[hypothesisKey].RevealEvent?.Invoke();
    }
}
