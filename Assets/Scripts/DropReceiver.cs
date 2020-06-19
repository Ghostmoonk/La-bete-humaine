using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropReceiver : MonoBehaviour, IDropHandler
{
    [SerializeField] List<string> acceptedTags;

    public event EventHandler<string> OnDropEvents;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Droped :" + Dragable.currentObjectDragged.tag);

        if (acceptedTags.Contains(Dragable.currentObjectDragged.tag))
        {
            OnDropEvents?.Invoke(this, Dragable.currentObjectDragged.GetComponent<TextMeshProUGUI>().text);
        }
    }

}
