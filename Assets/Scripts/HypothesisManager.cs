using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypothesisManager : MonoBehaviour
{
    [SerializeField] HypothesisData[] hypothesesData;
    [SerializeField] Hypothesis[] hypotheses;
    [SerializeField] HypothesisDisplayer displayer;
    private static HypothesisManager instance;
    public static HypothesisManager Instance
    {
        get
        {
            return instance;
        }
    }
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
        }
    }

    public Hypothesis[] GetHypothesis()
    {
        return hypotheses;
    }

    public void CompleteHypothesis(int hypothesisIndex)
    {
        hypotheses[hypothesisIndex].found = true;
        Debug.Log("CompleteH");
        displayer.RevealHypothesis(hypotheses[hypothesisIndex]);
    }
}

public class Hypothesis
{
    public Hypothesis(HypothesisData data)
    {
        this.data = data;
        found = false;
        discussed = discussed;
    }
    public HypothesisData data;
    public bool found;
    public bool discussed;
}