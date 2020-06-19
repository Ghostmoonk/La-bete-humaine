using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent BeginDragEvents;
    public UnityEvent EndDragEvents;
    public static GameObject currentObjectDragged;
    Vector3 screenPoint;

    Vector3 initialPos;
    Transform initialParent;

    GameObject instantedObject;
    int childIndex = -1;

    private void Start()
    {
        initialPos = transform.position;
        initialParent = transform.parent;

        if (transform.parent != null)
            childIndex = transform.GetSiblingIndex();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvents?.Invoke();
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        currentObjectDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        EndDragEvents?.Invoke();
        currentObjectDragged = null;
    }

    public void SetParentByTagName(string tag)
    {
        transform.parent = GameObject.FindGameObjectWithTag(tag).transform;
    }

    public void DuplicateObject()
    {
        int indexOfChild = transform.GetSiblingIndex();

        instantedObject = Instantiate(gameObject, gameObject.transform.parent);
        instantedObject.transform.SetSiblingIndex(indexOfChild);
    }

    public void RemoveInstantiateObject()
    {
        if (instantedObject != null)
        {
            Destroy(instantedObject);
        }
    }

    public void Replace()
    {
        transform.position = initialPos;
    }

    public void ResetParent()
    {
        transform.parent = initialParent;
        if (childIndex != -1)
            transform.SetSiblingIndex(childIndex);
    }

    public void DeleteObject(GameObject obj)
    {
        Destroy(obj);
    }
}
