using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomDecorSpawner : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] MovementReferenceObject followedObject;
    [SerializeField] RandomBackgroundSpeed[] randomBackgroundSpeed;
    float screenPointSide;

    [SerializeField] protected Vector2 direction;

    protected Vector2 screenBounds;

    private void Start()
    {
        screenBounds = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));
        screenPointSide = screenBounds.x / 2 + (screenBounds.x / 2 * -Mathf.Clamp(direction.x, -1, 1));

        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            item.randomDist = Random.Range(item.spawnRateIntervalInDistance.x, item.spawnRateIntervalInDistance.y);

            //item.timer.SetTimer(Random.Range(item.spawnRateIntervalInDistance.x, item.spawnRateIntervalInDistance.y));
            //item.timer.timerEndEvent.AddListener(delegate
            //{
            //    SpawnDecor(item);
            //    item.timer.SetTimer(Random.Range(item.spawnRateIntervalInDistance.x, item.spawnRateIntervalInDistance.y));
            //    item.timer.StartTimer();
            //});
            //item.timer.SetTimerActive(true);
            //item.timer.StartTimer();

        }
    }

    //Increase a counter and spawn a decor when it's time
    private void Update()
    {
        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            item.compteur += followedObject.speed * Time.deltaTime;

            if (item.compteur >= item.randomDist)
            {
                SpawnDecor(item);
                item.compteur = 0f;
            }
        }
    }

    //Move the random spawned decors
    private void LateUpdate()
    {
        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            foreach (Transform child in item.parent)
            {
                if (child.gameObject.activeInHierarchy)
                    child.transform.Translate(direction * item.bgSpeed.speed * Time.deltaTime * followedObject.speed);

                if (direction.x > 0 && child.transform.position.x - child.GetComponent<SpriteRenderer>().bounds.extents.x > screenPointSide)
                {
                    child.gameObject.SetActive(false);
                }
                else if (direction.x < 0 && child.transform.position.x + child.GetComponent<SpriteRenderer>().bounds.extents.x < -screenPointSide)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SpawnDecor(RandomBackgroundSpeed randomBg)
    {
        randomBg.randomDist = Random.Range(randomBg.spawnRateIntervalInDistance.x, randomBg.spawnRateIntervalInDistance.y);

        for (int i = 0; i < randomBg.parent.childCount; i++)
        {
            if (!randomBg.parent.GetChild(i).gameObject.activeInHierarchy)
            {
                if (randomBg.randomSprites.Length >= 2)
                    randomBg.parent.GetChild(i).GetComponent<SpriteRenderer>().sprite = randomBg.randomSprites[Random.Range(0, randomBg.randomSprites.Length)];

                randomBg.parent.GetChild(i).transform.position =
                    new Vector3((screenPointSide + randomBg.parent.GetChild(i).GetComponent<SpriteRenderer>().bounds.extents.x) * -direction.x, randomBg.parent.transform.position.y, randomBg.parent.transform.position.z);
                randomBg.parent.GetChild(i).gameObject.SetActive(true);
                return;
            }
        }
        GameObject objToInstantiate = Instantiate(randomBg.bgSpeed.backgroundGroup, randomBg.parent);
        objToInstantiate.transform.position = new Vector3((screenPointSide + objToInstantiate.GetComponent<SpriteRenderer>().bounds.extents.x) * -direction.x, randomBg.parent.transform.position.y, randomBg.parent.transform.position.z);
    }
}

[System.Serializable]
public class RandomBackgroundSpeed
{
    public BackgroundsSpeed bgSpeed;
    public Transform parent;
    public Vector2 spawnRateIntervalInDistance;
    [HideInInspector] public float randomDist;
    [HideInInspector] public float compteur = 0f;
    public Sprite[] randomSprites;

}
