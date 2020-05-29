using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] ObjectToMove[] objectsToMove;
    [SerializeField] float xLimit;

    private void Start()
    {
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            objectsToMove[i].renderers = objectsToMove[i].obj.GetComponentsInChildren<Renderer>();
        }
    }

    void Update()
    {
        MoveObjects();
    }

    private void MoveObjects()
    {
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            objectsToMove[i].obj.transform.Translate(objectsToMove[i].speed * objectsToMove[i].dir.normalized * Time.deltaTime);


            if (objectsToMove[i].obj.transform.position.x > xLimit)
            {
                objectsToMove[i].obj.transform.position = objectsToMove[i].replacePoint.position;
            }
        }
    }


    [System.Serializable]
    public struct ObjectToMove
    {
        public GameObject obj;
        public Transform replacePoint;
        [Range(0, 2f)]
        public float speed;
        public Vector3 dir;
        [HideInInspector] public Renderer[] renderers;
    }

}
