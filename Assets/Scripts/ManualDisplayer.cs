using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManualDisplayer : MonoBehaviour
{
    [SerializeField] GameObject summaryPage;
    [SerializeField] GameObject summaryItemPrefab;
    [SerializeField] Transform summaryItemParent;

    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;

    [SerializeField] Transform manualPageParent;
    [SerializeField] GameObject manualNotePrefab;
    [SerializeField] ManualNote[] manualNotesData;

    Dictionary<AnomalieType, List<GameObject>> pagesDico;
    AnomalieType currentAnomalieTypeDisplayed;
    int currentIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        pagesDico = new Dictionary<AnomalieType, List<GameObject>>();
        //List<AnomalieType> anomalieTypesRemaining = new List<AnomalieType>();

        foreach (ManualNote item in manualNotesData)
        {
            //if (!anomalieTypesRemaining.Contains(item.anomalieType))
            //{
            //    anomalieTypesRemaining.Add(item.anomalieType);
            //}

            InstantiateNote(item);
        }

        //InstantiateSummaryItems(anomalieTypesRemaining);
    }

    //void InstantiateSummaryItems(List<AnomalieType> parts)
    //{
    //    foreach (AnomalieType anomalyType in parts)
    //    {
    //        InstantiateSummaryItem(anomalyType);
    //    }
    //}

    void InstantiateSummaryItem(AnomalieType anomalyType)
    {
        GameObject itemSummary = Instantiate(summaryItemPrefab, summaryItemParent);
        itemSummary.GetComponentInChildren<TextMeshProUGUI>().text = anomalyType.ToString();
        itemSummary.GetComponent<Button>().onClick.AddListener(delegate { SetAnomalieTypeDisplaying(anomalyType); DesactivatePages(); ShowPage(0); });
    }

    private void ShowPage(int index)
    {
        pagesDico[currentAnomalieTypeDisplayed][index].SetActive(true);
        currentIndex = index;

        if (index >= pagesDico[currentAnomalieTypeDisplayed].Count - 1)
        {
            rightArrow.interactable = false;
        }
        else if (!rightArrow.interactable)
        {
            rightArrow.interactable = true;
        }

        if (!leftArrow.interactable)
        {
            leftArrow.interactable = true;
        }
    }

    private void DesactivatePages()
    {
        foreach (Transform page in manualPageParent)
        {
            if (page.gameObject.activeInHierarchy)
                page.gameObject.SetActive(false);
        }
    }

    public void DisplayNextPage(int increment)
    {
        currentIndex += increment;
        DesactivatePages();
        if (currentIndex >= 0)
        {
            ShowPage(currentIndex);
        }
        else
        {
            leftArrow.interactable = false;
            rightArrow.interactable = false;
            summaryPage.SetActive(true);
        }
    }

    private void SetAnomalieTypeDisplaying(AnomalieType anomalieType) => currentAnomalieTypeDisplayed = anomalieType;

    void InstantiateNote(ManualNote manualNote)
    {
        GameObject manualPage = Instantiate(manualNotePrefab, manualPageParent);
        if (!manualPage.GetComponent<ManualPage>())
            throw new MissingComponentException("Missing ManualPage Component on instanted Object");
        else
        {
            if (!pagesDico.ContainsKey(manualNote.anomalieType))
            {
                pagesDico.Add(manualNote.anomalieType, new List<GameObject>());
                InstantiateSummaryItem(manualNote.anomalieType);
            }

            pagesDico[manualNote.anomalieType].Add(manualPage);

            manualPage.GetComponent<ManualPage>().SetupContents(manualNote.anomalieType, manualNote.title, manualNote.text, manualNote.textParatext, manualNote.manuscrit, manualNote.manuscritParatext);
            manualPage.SetActive(false);
        }
        Resizer.ResizeLayout(manualPageParent.GetComponent<RectTransform>());
    }

}
