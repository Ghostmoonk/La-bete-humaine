using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HypothesisVerifier : MonoBehaviour
{
    [SerializeField] Hypothesis[] hypotheses;
    [SerializeField] TextMeshProUGUI textMesh;

    public void CheckHypothesis(string text)
    {
        for (int i = 0; i < hypotheses.Length; i++)
        {
            if (text.ToLower() == hypotheses[i].text.ToLower())
            {
                Debug.Log("Hypothèse trouvée");
            }
        }
    }
}
