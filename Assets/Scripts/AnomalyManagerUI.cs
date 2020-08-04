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

    [Header("Anomaly")]
    [SerializeField] GameObject anomalyAnswerPrefab;
    [SerializeField] GameObject anomalyQuestionBox;
    [SerializeField] Transform answersContainer;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Button submitButton;
    [SerializeField] Button skipButton;
    [Header("Verification")]
    [SerializeField] Sprite successIcon;
    [SerializeField] Sprite failIcon;
    [SerializeField] Color successColor;
    [SerializeField] Color failColor;
    [SerializeField] float delayBetweenVerificationIconDisplay;
    [SerializeField] UnityEvent OnVerificationOver;

    [Header("Gauges")]
    [SerializeField] Ease changingGaugeValueEase;
    [Tooltip("Of how much the transition time in the gauge is multiplicated")]
    [SerializeField] float changingGaugeTimeMultiplicator = 1f;
    [SerializeField] Image charcoalGauge;
    [SerializeField] float maxCharcoalScore;
    float currentCharcoalScore;
    [SerializeField] Image timeGauge;
    [SerializeField] float maxTime;
    float currentTimeScoreScore;

    Dictionary<ScriptableAnswer, AnomalyAnswer> answersToggle;

    [SerializeField] UnityEventLocomotiveData OnContentSet;

    private void Start()
    {
        #region Score
        currentCharcoalScore = maxCharcoalScore;
        currentTimeScoreScore = maxTime;
        #endregion

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

        answersToggle = new Dictionary<ScriptableAnswer, AnomalyAnswer>();
    }

    public void SetAnomalyContent(LocomotiveAnomalyData data)
    {
        questionText.text = data.questionText;

        //We clear the list and destroy all childs if there are
        answersToggle.Clear();
        foreach (Transform child in answersContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ScriptableAnswer answer in data.answers)
        {
            GameObject answerToInstantiate = Instantiate(anomalyAnswerPrefab, answersContainer);
            answersToggle.Add(answer, answerToInstantiate.GetComponent<AnomalyAnswer>());
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;

            answerToInstantiate.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate
            {
                ToggleSubmitButtonInteractable();
            });
        }

        //OnContentSet?.Invoke(data);

    }

    public void ResizeRectByParent(RectTransform parent)
    {
        Resizer.ResizeLayout(parent.GetComponent<RectTransform>());
    }

    private void ToggleSubmitButtonInteractable()
    {
        foreach (KeyValuePair<ScriptableAnswer, AnomalyAnswer> item in answersToggle)
        {
            if (item.Value.GetToggle().isOn)
            {
                submitButton.interactable = true;
                return;
            }
        }
        submitButton.interactable = false;

    }

    //Display the marker on board
    public void ShowMarker()
    {
        LocomotiveAnomalyData data;

        if (AnomalyManager.Instance.GetCurrentAnomaly() != null)
            data = AnomalyManager.Instance.GetCurrentAnomaly();
        else
            return;

        board.GetComponent<ToggleMover>().EndToggleOn.RemoveAllListeners();
        board.GetComponent<ToggleMover>().EndToggleOff.RemoveAllListeners();
        Button marqueurButton = marqueur.GetComponentInChildren<Button>();

        if (board.GetComponent<ToggleMover>())
        {
            ToggleMover boardToggler = board.GetComponent<ToggleMover>();
            Vector3[] corners = new Vector3[4];
            retractor.GetWorldCorners(corners);
            //If the board toggler is not toggled, we display it on the retractor
            if (!boardToggler.GetToggler() && boardComponentsDico.ContainsKey(data.locomotiveComponent))
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

        foreach (KeyValuePair<ScriptableAnswer, AnomalyAnswer> answerToggle in answersToggle)
        {
            answersBool.Add(answerToggle.Key, answerToggle.Value.GetToggle().isOn);
            answerToggle.Value.GetToggle().interactable = false;
        }

        AnomalyManager.Instance.VerifyAnswers(answersBool);
    }

    public void DecreaseGauges(LocomotiveAnomalyData anomalyData)
    {
        currentCharcoalScore -= anomalyData.charcoalCostFail;
        currentTimeScoreScore -= anomalyData.timeCostFail;
        StartCoroutine(DecreaseGaugeSmoothly(charcoalGauge, currentCharcoalScore / maxCharcoalScore, changingGaugeValueEase));
        StartCoroutine(DecreaseGaugeSmoothly(timeGauge, currentTimeScoreScore / maxTime, changingGaugeValueEase));
    }

    IEnumerator DecreaseGaugeSmoothly(Image img, float newValue, Ease ease)
    {
        float currentValue = img.fillAmount;
        float timeToAnimate = newValue * changingGaugeTimeMultiplicator;
        Tween tween = DOTween.To(() => currentValue, x => currentValue = x, newValue, timeToAnimate).SetEase(ease);
        while (timeToAnimate > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            img.fillAmount = currentValue;
            Debug.Log(tween.IsComplete());
            timeToAnimate -= Time.deltaTime;
        }
        StopCoroutine(DecreaseGaugeSmoothly(img, newValue, ease));
    }

    public void ShowVerification(Dictionary<ScriptableAnswer, bool> correctionDico, bool succeed)
    {
        StartCoroutine(ShowVerificationDelayed(correctionDico, succeed));
    }

    public IEnumerator ShowVerificationDelayed(Dictionary<ScriptableAnswer, bool> correctionDico, bool succeed)
    {
        foreach (KeyValuePair<ScriptableAnswer, AnomalyAnswer> item in answersToggle)
        {
            item.Value.GetResultIconImage().gameObject.SetActive(true);
            if (correctionDico[item.Key])
            {
                item.Value.UpdateSprite(successIcon, successColor);
            }
            else
            {
                item.Value.UpdateSprite(failIcon, failColor);
            }

            item.Value.gameObject.GetComponentInChildren<GraphicFader>().FadeIn(1f);

            yield return new WaitForSeconds(delayBetweenVerificationIconDisplay);
        }

        OnVerificationOver?.Invoke();

        StopCoroutine(ShowVerificationDelayed(correctionDico, succeed));

        anomalyQuestionBox.GetComponent<GraphicFader>().EndFadeOutText.RemoveAllListeners();
        if (succeed)
            anomalyQuestionBox.GetComponent<GraphicFader>().EndFadeOutText.AddListener(delegate { AnomalyManager.Instance.GetAnomalyEvents(AnomalyManager.Instance.GetCurrentAnomaly()).OnResolveSuccess?.Invoke(); });
        else
        {
            Debug.Log(AnomalyManager.Instance.GetCurrentAnomaly());
            anomalyQuestionBox.GetComponent<GraphicFader>().EndFadeOutText.AddListener(delegate { AnomalyManager.Instance.GetAnomalyEvents(AnomalyManager.Instance.GetCurrentAnomaly()).OnResolveFail?.Invoke(); });
        }

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
