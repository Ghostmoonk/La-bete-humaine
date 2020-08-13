using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI charcoalText;
    [SerializeField] TextMeshProUGUI timeText;

    public void SetScore()
    {
        charcoalText.text = (AnomalyManagerUI.Instance.GetScoreCharcoalRatio() * 100f).ToString() + "%";
        timeText.text = (AnomalyManagerUI.Instance.GetScoreTimeRatio() * 100f).ToString() + "%";
    }
}
