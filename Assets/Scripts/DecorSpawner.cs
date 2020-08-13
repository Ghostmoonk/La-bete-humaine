using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] MovementReferenceObject followedObject;
    [SerializeField] float clearLimit;
    [Tooltip("Those objects will spawn randomly everytime")]
    [SerializeField] RandomBackgroundSpeed[] randomBackgroundSpeed;

    [SerializeField] RandomBackgroundSpeed[] ponctualBackgrounds;
    Dictionary<Transform, RandomBackgroundSpeed> ponctualBackgroundDico;
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

        ponctualBackgroundDico = new Dictionary<Transform, RandomBackgroundSpeed>();

        foreach (var item in ponctualBackgrounds)
        {
            item.bgSpeed.ownSpeed = Random.Range(item.bgSpeed.ownSpeedInterval.x, item.bgSpeed.ownSpeedInterval.y);
            ponctualBackgroundDico.Add(item.parent, item);
        }
    }

    //Increase a counter and spawn a decor when rnadom distance is reached
    private void Update()
    {
        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            item.compteur += Mathf.Abs(followedObject.speed) * Time.deltaTime;

            if (item.compteur >= item.randomDist)
            {
                SpawnDecor(item);
                item.compteur = 0f;
            }
        }

        direction.x = followedObject.speed > 0 ? -Mathf.Abs(direction.x) : Mathf.Abs(direction.x);
    }

    private Vector3 GetSpawnPositionFromSprite(SpriteRenderer spriteR, int direction)
    {
        return new Vector3(
            (screenPointSide + spriteR.bounds.extents.x) * -direction,
            spriteR.gameObject.transform.position.y,
            spriteR.gameObject.transform.position.z
            );
    }

    public void SpawnPonctualBackground(Transform parent)
    {
        RandomBackgroundSpeed currentBg = ponctualBackgroundDico[parent];

        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.GetComponent<SpriteRenderer>().sprite = currentBg.randomSprites[Random.Range(0, currentBg.randomSprites.Length)];
                child.transform.position = GetSpawnPositionFromSprite(child.GetComponent<SpriteRenderer>(), currentBg.bgSpeed.relativeSpeed * direction.x > 0 ? (int)-direction.x : (int)direction.x);
                child.gameObject.SetActive(true);

                return;
            }
        }

        GameObject ponctualBgToInstantiate = Instantiate(currentBg.bgSpeed.backgroundGroup, parent);
        ponctualBgToInstantiate.GetComponent<SpriteRenderer>().sprite = currentBg.randomSprites[Random.Range(0, currentBg.randomSprites.Length - 1)];
        ponctualBgToInstantiate.GetComponent<SpriteRenderer>().sortingOrder = currentBg.sortingOrder;
        ponctualBgToInstantiate.transform.position = GetSpawnPositionFromSprite(ponctualBgToInstantiate.GetComponent<SpriteRenderer>(), currentBg.bgSpeed.relativeSpeed * direction.x > 0 ? (int)-direction.x : (int)direction.x);

    }

    //Obsolete
    //public void SpawnPonctualBackground(Sprite sprite)
    //{
    //    foreach (BackgroundsSpeed ponctualBg in ponctualBackgroundList)
    //    {
    //        if (!ponctualBg.backgroundGroup.activeInHierarchy)
    //        {
    //            ponctualBg.backgroundGroup.GetComponent<SpriteRenderer>().sprite = sprite;
    //            ponctualBg.backgroundGroup.transform.position = GetSpawnPositionFromSprite(ponctualBg.backgroundGroup.GetComponent<SpriteRenderer>());
    //            ponctualBg.backgroundGroup.SetActive(true);

    //            return;
    //        }
    //    }

    //    GameObject ponctualBgToInstantiate = Instantiate(ponctualBackgroundTemplate.backgroundGroup, ponctualBackgroundParent);
    //    ponctualBgToInstantiate.GetComponent<SpriteRenderer>().sprite = sprite;

    //    ponctualBgToInstantiate.transform.position = GetSpawnPositionFromSprite(ponctualBgToInstantiate.GetComponent<SpriteRenderer>());

    //    BackgroundsSpeed bgSpeed = new BackgroundsSpeed();
    //    bgSpeed.backgroundGroup = ponctualBgToInstantiate;
    //    bgSpeed.speed = ponctualBackgroundTemplate.speed;

    //    ponctualBackgroundList.Add(bgSpeed);

    //}

    private void CheckDesactivateObjectOutOfCamera(Transform obj)
    {
        if (direction.x > 0 && obj.position.x - obj.GetComponent<SpriteRenderer>().bounds.extents.x > screenPointSide)
            obj.gameObject.SetActive(false);

        else if (direction.x < 0 && obj.position.x + obj.GetComponent<SpriteRenderer>().bounds.extents.x < -screenPointSide)
            obj.gameObject.SetActive(false);

    }

    private void CheckDesactivateMovingPoncutalDecor(Transform obj)
    {
        if (obj.position.x > clearLimit * -direction.x)
        {
            obj.gameObject.SetActive(false);
        }
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
                    child.transform.Translate(direction * item.bgSpeed.relativeSpeed * Time.deltaTime * Mathf.Abs(followedObject.speed));

                CheckDesactivateObjectOutOfCamera(child);
            }
        }

        //Move ponctual decor
        foreach (KeyValuePair<Transform, RandomBackgroundSpeed> item in ponctualBackgroundDico)
        {
            for (int i = 0; i < item.Key.childCount; i++)
            {
                if (item.Key.GetChild(i).gameObject.activeInHierarchy)
                {
                    item.Key.GetChild(i).Translate((direction * item.Value.bgSpeed.relativeSpeed * followedObject.speed + new Vector2(item.Value.bgSpeed.ownSpeed, 0)) * Time.deltaTime);
                }

                CheckDesactivateObjectOutOfCamera(item.Key.GetChild(i));

                if (item.Value.bgSpeed.ownSpeed != 0f)
                    CheckDesactivateMovingPoncutalDecor(item.Key.GetChild(i));
            }
        }
        //foreach (BackgroundsSpeed bg in ponctualBackgroundList)
        //{
        //    if (bg.backgroundGroup.activeInHierarchy)
        //        bg.backgroundGroup.transform.Translate(direction * bg.speed * Time.deltaTime * followedObject.speed);

        //    CheckDesactivateObjectOutOfCamera(bg.backgroundGroup.transform);
        //}
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
        objToInstantiate.GetComponent<SpriteRenderer>().sortingOrder = randomBg.sortingOrder;
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
    public int sortingOrder;

}
