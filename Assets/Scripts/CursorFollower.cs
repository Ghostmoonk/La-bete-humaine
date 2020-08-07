using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    Vector3 screenPoint;
    Transform followingTransform;
    float cursorListenerZ;

    private void Start()
    {
        followingTransform = GetComponent<Transform>();
        cursorListenerZ = Camera.main.WorldToScreenPoint(transform.position).z;
        screenPoint = Camera.main.WorldToScreenPoint(followingTransform.transform.position);

    }
    private void Update()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        followingTransform.transform.position = curPosition;
    }
}
