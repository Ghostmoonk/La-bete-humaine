using System.Collections;
using System.Collections.Generic;
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

    private void OnGUI()
    {
        Debug.Log("gui");
    }

    public void SetAnomalyContent(LocomotiveAnomalieData data)
    {

    }

}

[System.Serializable]
public struct BoardComponent
{
    public LocomotiveComponent locomotiveComponent;
    public Transform componentTransform;
}
