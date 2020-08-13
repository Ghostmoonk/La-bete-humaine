using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInfosUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI locoComponentText;

    public void SetHoverComponents(Dictionary<GameObject, LocomotiveComponent> dicoData)
    {
        foreach (KeyValuePair<GameObject, LocomotiveComponent> item in dicoData)
        {
            if (item.Key.GetComponent<EventTrigger>())
            {
                EventTrigger trigger = item.Key.GetComponent<EventTrigger>();

                EventTrigger.Entry entryEnter = new EventTrigger.Entry();
                EventTrigger.Entry entryExit = new EventTrigger.Entry();
                entryEnter.eventID = EventTriggerType.PointerEnter;
                entryExit.eventID = EventTriggerType.PointerExit;

                entryEnter.callback.AddListener(delegate { ShowComponentText(item.Value.ToString()); });
                entryExit.callback.AddListener(delegate { HideComponentText(); });

                trigger.triggers.Add(entryEnter);
                trigger.triggers.Add(entryExit);
            }
        }
    }

    private void ShowComponentText(string text)
    {
        string newText = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (i > 0 && char.IsUpper(c))
            {
                newText += " ";
                c = c.ToString().ToLower()[0];
            }
            newText += c;
        }
        locoComponentText.text = text;
        locoComponentText.gameObject.SetActive(true);
    }

    private void HideComponentText()
    {
        locoComponentText.gameObject.SetActive(false);
    }
}
