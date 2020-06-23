using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

//Make TextMeshProUGUI texts spawning & converging to the convergencePoint
public class ConvergingTexts : MonoBehaviour
{
    [SerializeField] Transform convergencePoint;
    [SerializeField] Collider2D appearanceArea;
    [SerializeField] TextMeshProUGUI[] objects;
    [SerializeField] float spawnFrequency;

    bool shouldSpawnObjects;
    [SerializeField] float convergeDuration;
    [SerializeField] float convergeDurationVariance;

    [SerializeField] bool faceTarget;

    private void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].gameObject.SetActive(false);
        }
    }

    public void Converge()
    {
        shouldSpawnObjects = true;
        StartCoroutine(SpawnCo());
    }

    public void StopConverge()
    {
        shouldSpawnObjects = false;
        StopCoroutine(SpawnCo());
    }
    IEnumerator SpawnCo()
    {
        while (shouldSpawnObjects)
        {
            int activeObjects = 0;
            for (int i = 0; i < objects.Length; i++)
                if (objects[i].gameObject.activeSelf)
                    activeObjects++;

            if (activeObjects < objects.Length)
                Spawn();

            yield return new WaitForSeconds(1 / spawnFrequency);
        }
    }

    private void Spawn()
    {
        Vector3 randomPos;
        do
        {
            randomPos = new Vector3(
                 Random.Range(appearanceArea.bounds.min.x, appearanceArea.bounds.max.x),
                 Random.Range(appearanceArea.bounds.min.y, appearanceArea.bounds.max.y),
                 Random.Range(appearanceArea.bounds.min.z, appearanceArea.bounds.max.z));
        } while (!appearanceArea.bounds.Contains(randomPos));


        TextMeshProUGUI objectToActivate;
        do
        {
            objectToActivate = objects[Random.Range(0, objects.Length)];
        } while (objectToActivate.gameObject.activeSelf);
        objectToActivate.gameObject.SetActive(true);
        objectToActivate.transform.position = randomPos;
        float randomDuration = Random.Range(convergeDuration - convergeDurationVariance, convergeDuration + convergeDurationVariance);
        MoveToTarget(objectToActivate, randomDuration);
        FadeOut(objectToActivate, randomDuration - randomDuration / 3);
    }

    private void MoveToTarget(TextMeshProUGUI obj, float duration)
    {
        Tweener moveTweener = obj.transform.DOMove(convergencePoint.position, duration);
        moveTweener.OnComplete(() => Desactivate(obj));
        //Invoke(nameof(Desactivate), randomDuration);
        if (faceTarget)
            FaceTarget(obj.transform);
    }

    private void FadeOut(TextMeshProUGUI text, float duration)
    {
        text.DOColor(new Color(text.color.r, text.color.g, text.color.b, 0f), duration);
    }

    private void Desactivate(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(false);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }

    private void FaceTarget(Transform t)
    {
        Vector3 dir = convergencePoint.position - t.position;
        t.eulerAngles = Vector3.zero;

        if (Vector3.Cross(dir.normalized, t.up).z > 0)
            t.eulerAngles = new Vector3(0f, 0f, 90f - Vector3.Angle(t.up, dir));
        else if (Vector3.Cross(dir.normalized, t.up).z < 0)
            t.eulerAngles = new Vector3(0f, 0f, 270f + Vector3.Angle(t.up, dir));
    }
}
