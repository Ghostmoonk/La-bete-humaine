using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] MovementReferenceObject followedObject;
    [SerializeField] BackgroundsSpeed[] backgroundsSpeed;
    [SerializeField] protected Vector2 direction;
    bool marching = true;

    protected Vector2 screenBounds;

    private void Start()
    {
        screenBounds = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));
        foreach (BackgroundsSpeed item in backgroundsSpeed)
        {
            LoadChildBackgrounds(item);
        }
    }

    //Create the backgrounds needed in order to have a propper parallax and movement effect
    void LoadChildBackgrounds(BackgroundsSpeed background)
    {
        float backgroundWidth = background.backgroundGroup.GetComponent<SpriteRenderer>().bounds.size.x;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 3 / backgroundWidth);
        GameObject clone = Instantiate(background.backgroundGroup);

        for (int i = 0; i < childsNeeded; i++)
        {
            GameObject c = Instantiate(clone, background.backgroundGroup.transform);
            c.transform.position = new Vector3(backgroundWidth * i, background.backgroundGroup.transform.position.y, background.backgroundGroup.transform.position.z);
            c.name = background.backgroundGroup.name + "- " + i;

            if (background.alternate && i % 2 == 0)
            {
                c.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        Destroy(clone);
        foreach (Component comp in background.backgroundGroup.GetComponents<Component>())
        {
            if (comp.GetType() != typeof(Transform))
            {
                Destroy(comp);
            }
        }
    }

    private void LateUpdate()
    {
        if (marching)
            //For each background group we want to move
            foreach (BackgroundsSpeed bgs in backgroundsSpeed)
            {
                float halfFirstBgWidth = bgs.backgroundGroup.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
                GameObject firstChild = bgs.backgroundGroup.transform.GetChild(0).gameObject;
                GameObject lastChild = bgs.backgroundGroup.transform.GetChild(bgs.backgroundGroup.transform.childCount - 1).gameObject;
                //For every child inside, which are repetitions of the same background
                foreach (Transform bg in bgs.backgroundGroup.transform)
                {
                    bg.Translate(direction * bgs.speed * Time.deltaTime * followedObject.speed);
                }

                if (transform.position.x - screenBounds.x > firstChild.transform.position.x + halfFirstBgWidth)
                {
                    firstChild.transform.SetAsLastSibling();
                    firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfFirstBgWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
                }
                else if (transform.position.x + screenBounds.x < lastChild.transform.position.x - halfFirstBgWidth)
                {
                    lastChild.transform.SetAsFirstSibling();
                    lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfFirstBgWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);

                }
            }

    }

}

[System.Serializable]
public struct BackgroundsSpeed
{
    public float speed;
    public GameObject backgroundGroup;

    public bool alternate;
}
