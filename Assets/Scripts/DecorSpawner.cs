using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] MovementReferenceObject followedObject;
    [Tooltip("Those objects will spawn randomly everytime")]
    [SerializeField] RandomBackgroundSpeed[] randomBackgroundSpeed;

    [SerializeField] Transform ponctualBackgroundParent;
    [SerializeField] BackgroundsSpeed ponctualBackgroundTemplate;
    List<BackgroundsSpeed> ponctualBackgroundList = new List<BackgroundsSpeed>();
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

        }
    }

    //Increase a counter and spawn a decor when rnadom distance is reached
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
    public void SetPoncutalBgTemplateSpeed(float newSpeed) => ponctualBackgroundTemplate.speed = newSpeed;

    private Vector3 GetSpawnPositionFromSprite(SpriteRenderer spriteR)
    {
        return new Vector3(
            (screenPointSide + spriteR.bounds.extents.x) * -direction.x,
            spriteR.gameObject.transform.position.y,
            spriteR.gameObject.transform.position.z
            );
    }

    public void SpawnPonctualBackground(Sprite sprite)
    {
        foreach (BackgroundsSpeed ponctualBg in ponctualBackgroundList)
        {
            if (!ponctualBg.backgroundGroup.activeInHierarchy)
            {
                ponctualBg.backgroundGroup.GetComponent<SpriteRenderer>().sprite = sprite;
                ponctualBg.backgroundGroup.transform.position = GetSpawnPositionFromSprite(ponctualBg.backgroundGroup.GetComponent<SpriteRenderer>());
                ponctualBg.backgroundGroup.SetActive(true);

                return;
            }
        }

        GameObject ponctualBgToInstantiate = Instantiate(ponctualBackgroundTemplate.backgroundGroup, ponctualBackgroundParent);
        ponctualBgToInstantiate.GetComponent<SpriteRenderer>().sprite = sprite;

        ponctualBgToInstantiate.transform.position = GetSpawnPositionFromSprite(ponctualBgToInstantiate.GetComponent<SpriteRenderer>());

        BackgroundsSpeed bgSpeed = new BackgroundsSpeed();
        bgSpeed.backgroundGroup = ponctualBgToInstantiate;
        bgSpeed.speed = ponctualBackgroundTemplate.speed;

        ponctualBackgroundList.Add(bgSpeed);

    }

    private void CheckDesactivateObjectOutOfCamera(Transform obj)
    {
        if (direction.x > 0 && obj.position.x - obj.GetComponent<SpriteRenderer>().bounds.extents.x > screenPointSide)
            obj.gameObject.SetActive(false);

        else if (direction.x < 0 && obj.position.x + obj.GetComponent<SpriteRenderer>().bounds.extents.x < -screenPointSide)
            obj.gameObject.SetActive(false);

    }


    //Move the random spawned decors
    private void LateUpdate()
    {
        //Move the random decor
        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            foreach (Transform child in item.parent)
            {
                if (child.gameObject.activeInHierarchy)
                    child.transform.Translate(direction * item.bgSpeed.speed * Time.deltaTime * followedObject.speed);

                CheckDesactivateObjectOutOfCamera(child);
            }
        }

        //Move the ponctual decor
        foreach (BackgroundsSpeed bg in ponctualBackgroundList)
        {
            if (bg.backgroundGroup.activeInHierarchy)
                bg.backgroundGroup.transform.Translate(direction * bg.speed * Time.deltaTime * followedObject.speed);

            CheckDesactivateObjectOutOfCamera(bg.backgroundGroup.transform);
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
