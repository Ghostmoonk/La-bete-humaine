using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecorSpawner : MonoBehaviour
{
    [SerializeField] Camera mainCam;
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
            item.timer.SetTimer(Random.Range(item.spawnRateIntervalInSeconds.x, item.spawnRateIntervalInSeconds.y));
            item.timer.timerEndEvent.AddListener(delegate
            {
                SpawnDecor(item);
                item.timer.SetTimer(Random.Range(item.spawnRateIntervalInSeconds.x, item.spawnRateIntervalInSeconds.y));
                item.timer.ResetTimer();
                item.timer.StartTimer();
            });
            item.timer.SetTimerActive(true);
            item.timer.StartTimer();
        }
    }

    private void LateUpdate()
    {
        foreach (RandomBackgroundSpeed item in randomBackgroundSpeed)
        {
            foreach (Transform child in item.parent)
            {
                if (child.gameObject.activeInHierarchy)
                    child.transform.Translate(direction * item.bgSpeed.speed * Time.deltaTime);

                if (direction.x > 0 && child.transform.position.x + child.GetComponent<SpriteRenderer>().bounds.extents.x > screenPointSide)
                {
                    //child.gameObject.SetActive(false);
                }
                else if (direction.x < 0 && child.transform.position.x - child.GetComponent<SpriteRenderer>().bounds.extents.x < screenPointSide)
                {
                    //child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SpawnDecor(RandomBackgroundSpeed randomBg)
    {
        Debug.Log("should spawn");
        for (int i = 0; i < randomBg.parent.childCount; i++)
        {
            if (!randomBg.parent.GetChild(i).gameObject.activeInHierarchy)
            {
                randomBg.parent.GetChild(i).transform.position =
                    new Vector3(screenPointSide, randomBg.parent.transform.position.y, randomBg.parent.transform.position.z);
                randomBg.parent.GetChild(i).gameObject.SetActive(true);

                return;
            }
        }
        GameObject objToInstantiate = Instantiate(randomBg.bgSpeed.backgroundGroup, randomBg.parent);
        objToInstantiate.transform.position = new Vector3(screenPointSide, randomBg.parent.transform.position.y, randomBg.parent.transform.position.z);

    }
}

[System.Serializable]
public struct RandomBackgroundSpeed
{
    public BackgroundsSpeed bgSpeed;
    public Transform parent;
    public Vector2 spawnRateIntervalInSeconds;
    public Timer timer;
}
