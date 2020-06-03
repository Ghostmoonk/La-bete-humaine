using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Speed
{
    Normal, Fast
}

public class Mover : MonoBehaviour
{
    [SerializeField] ObjectToMove[] objectsToMove;
    Speed speedType;

    private void Start()
    {
        speedType = Speed.Normal;
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
            if (speedType == Speed.Normal)
                objectsToMove[i].obj.transform.Translate(objectsToMove[i].normalSpeed * objectsToMove[i].dir.normalized * Time.deltaTime);
            else
                objectsToMove[i].obj.transform.Translate(objectsToMove[i].fastSpeed * objectsToMove[i].dir.normalized * Time.deltaTime);

            if (objectsToMove[i].obj.transform.position.x > objectsToMove[i].horizontalLimit)
                objectsToMove[i].obj.transform.position = objectsToMove[i].replacePoint.position;
        }
    }

    public void SetSpeedType(int speedType) => this.speedType = (Speed)speedType;


    [System.Serializable]
    public struct ObjectToMove
    {
        public GameObject obj;
        public Transform replacePoint;
        [Range(0, 2f)]
        public float normalSpeed;
        public float fastSpeed;
        public Vector3 dir;
        [HideInInspector] public Renderer[] renderers;
        public float horizontalLimit;
    }

}
