using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HypothesisVerifier : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    public void CheckHypothesis(string text)
    {
        for (int i = 0; i < HypothesisManager.Instance.GetHypothesis().Length; i++)
        {
            if (text.ToLower() == HypothesisManager.Instance.GetHypothesis()[i].data.text.ToLower())
            {
                Debug.Log("Hypothèse trouvée");
            }
        }
    }
}
