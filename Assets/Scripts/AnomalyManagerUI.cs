using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnomalyManagerUI : MonoBehaviour
{
    private static AnomalyManagerUI instance;
    public static AnomalyManagerUI Instance
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
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
    }

    [Header("Board")]
    [SerializeField] RectTransform board;
    [SerializeField] RectTransform marqueur;
    [SerializeField] BoardComponent[] boardComponents;

    [Header("Anomalie")]
    [SerializeField] GameObject anomalyAnswerPrefab;
    [SerializeField] GameObject anomalyQuestionBox;
    [SerializeField] Transform answersContainer;
    [SerializeField] TextMeshProUGUI questionText;

    Dictionary<ScriptableAnswer, Toggle> answersToggle;

    private void Start()
    {
        answersToggle = new Dictionary<ScriptableAnswer, Toggle>();
    }

    public void SetAnomalyContent(LocomotiveAnomalyData data)
    {
        questionText.text = data.questionText;

        answersToggle.Clear();

        foreach (ScriptableAnswer answer in data.answers)
        {
            GameObject answerToInstantiate = Instantiate(anomalyAnswerPrefab, answersContainer);
            answersToggle.Add(answer, answerToInstantiate.GetComponentInChildren<Toggle>());
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;
        }
    }

    public void RetrieveAnswers()
    {
        Dictionary<ScriptableAnswer, bool> answersBool = new Dictionary<ScriptableAnswer, bool>();

        foreach (KeyValuePair<ScriptableAnswer, Toggle> answerToggle in answersToggle)
        {
            answersBool.Add(answerToggle.Key, answerToggle.Value.isOn);
        }

        AnomalyManager.Instance.VerifyAnswers(answersBool);
    }

}

[System.Serializable]
public struct BoardComponent
{
    public LocomotiveComponent locomotiveComponent;
    public Transform componentTransform;
}
