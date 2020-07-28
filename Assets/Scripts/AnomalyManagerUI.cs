using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] Canvas mainCanvas;
    [Header("Board")]
    [SerializeField] BoardInfosUI boardInfosUI;
    [SerializeField] RectTransform board;
    [SerializeField] RectTransform marqueur;
    [SerializeField] RectTransform retractor;
    [SerializeField] BoardComponent[] boardComponents;
    Dictionary<LocomotiveComponent, Transform> boardComponentsDico;
    [Range(0, 2f)]
    [SerializeField] float markerSlidingDuration;

    [Header("Anomalie")]
    [SerializeField] GameObject anomalyAnswerPrefab;
    [SerializeField] GameObject anomalyQuestionBox;
    [SerializeField] Transform answersContainer;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Button submitButton;

    Dictionary<ScriptableAnswer, Toggle> answersToggle;

    [SerializeField] UnityEventLocomotiveData OnContentSet;

    private void Start()
    {
        boardComponentsDico = new Dictionary<LocomotiveComponent, Transform>();

        Dictionary<GameObject, LocomotiveComponent> locoCompDuico = new Dictionary<GameObject, LocomotiveComponent>();

        for (int i = 0; i < boardComponents.Length; i++)
        {
            if (!boardComponentsDico.ContainsKey(boardComponents[i].locomotiveComponent))
            {
                boardComponentsDico.Add(boardComponents[i].locomotiveComponent, boardComponents[i].componentTransform);

                locoCompDuico.Add(boardComponents[i].componentTransform.gameObject, boardComponents[i].locomotiveComponent);
            }
        }

        boardInfosUI.SetHoverComponents(locoCompDuico);

        answersToggle = new Dictionary<ScriptableAnswer, Toggle>();
    }

    public void SetAnomalyContent(LocomotiveAnomalyData data)
    {
        questionText.text = data.questionText;

        //We clear the list and destroy all childs if there are
        answersToggle.Clear();
        foreach (Transform child in answersContainer)
        {
            Destroy(child);
        }

        foreach (ScriptableAnswer answer in data.answers)
        {
            GameObject answerToInstantiate = Instantiate(anomalyAnswerPrefab, answersContainer);
            answersToggle.Add(answer, answerToInstantiate.GetComponentInChildren<Toggle>());
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;

            answerToInstantiate.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate
            {
                ToggleSubmitButtonInteractable();
            });
        }

        OnContentSet?.Invoke(data);

        Resizer.ResizeLayout(anomalyQuestionBox.GetComponent<RectTransform>());

    }

    private void ToggleSubmitButtonInteractable()
    {
        foreach (KeyValuePair<ScriptableAnswer, Toggle> item in answersToggle)
        {
            if (item.Value.isOn)
            {
                submitButton.interactable = true;
                return;
            }
        }
        submitButton.interactable = false;

    }

    //Display the marker on board
    public void ShowMarker(LocomotiveAnomalyData data)
    {
        board.GetComponent<ToggleMover>().EndToggleOn.RemoveAllListeners();
        board.GetComponent<ToggleMover>().EndToggleOff.RemoveAllListeners();
        Button marqueurButton = marqueur.GetComponentInChildren<Button>();

        if (board.GetComponent<ToggleMover>())
        {
            ToggleMover boardToggler = board.GetComponent<ToggleMover>();
            Vector3[] corners = new Vector3[4];
            retractor.GetWorldCorners(corners);
            //If the board toggler is not toggled, we display it on the retractor
            if (!boardToggler.GetToggler())
            {
                marqueur.transform.position = corners[2];
                marqueur.gameObject.SetActive(true);
                SetInteractable(marqueurButton, false);
            }
            else
            {
                if (boardComponentsDico.ContainsKey(data.locomotiveComponent))
                {
                    marqueur.transform.position = boardComponentsDico[data.locomotiveComponent].position;
                    marqueur.gameObject.SetActive(true);
                    SetInteractable(marqueurButton, true);
                }
            }
            board.GetComponent<ToggleMover>().EndToggleOn.AddListener(delegate { SlideMarkerToPosition(boardComponentsDico[data.locomotiveComponent].position, markerSlidingDuration); SetInteractable(marqueurButton, true); });
            board.GetComponent<ToggleMover>().EndToggleOff.AddListener(delegate { SlideMarkerToPosition(corners[2], markerSlidingDuration); SetInteractable(marqueurButton, false); });
        }
    }

    private void SlideMarkerToPosition(Vector3 position, float duration)
    {
        marqueur.DOKill();
        marqueur.DOMove(position, duration);
    }

    private void SetInteractable(Selectable selectable, bool interactable)
    {
        selectable.interactable = interactable;
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

[Serializable]
public class UnityEventLocomotiveData : UnityEvent<LocomotiveAnomalyData>
{

}
