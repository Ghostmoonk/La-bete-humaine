using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementReferenceObject : MonoBehaviour
{
    public float speed;
    [SerializeField] float timeToChangeSpeed;
    [SerializeField] Ease accelerationCurve;
    [HideInInspector] public float distanceDone;
    bool consideredAsMoving = true;

    public void StopInTime(float stopDuration)
    {
        DOTween.To(() => speed, x => speed = x, 0f, stopDuration);
    }

    public void ChangeSpeed(float newSpeed)
    {
        DOTween.To(() => speed, x => speed = x, newSpeed, timeToChangeSpeed).SetEase(accelerationCurve);
    }

    public void SetTimeToChangeSpeed(float newAccelerationTime)
    {
        timeToChangeSpeed = newAccelerationTime;
    }

    public void SlowStopToDistance(float ratioDist)
    {
        Debug.Log("Debut slow, distance parcourue :" + distanceDone);
        Debug.Log("Equivalent ratio : " + Mathf.Lerp(0, LocomotiveRouteManager.Instance.GetCurrentStationInRoute().distwithNextStation, ratioDist));


        //Tween tween = DOTween.To(() => speed, x => speed = x, 0f, realDist / 2 * Mathf.Pow(iniSpeed, 2));

        float realDist = ratioDist * LocomotiveRouteManager.Instance.GetCurrentStationInRoute().distwithNextStation;
        StartCoroutine(SlowStop(realDist));


        //tween.OnComplete(() => Debug.Log("parcouru : " + distanceDone));
    }

    IEnumerator SlowStop(float distance)
    {
        float iniSpeed = speed;

        float acceleration = -Mathf.Pow(iniSpeed, 2) / (2 * distance);
        float iniTime = -iniSpeed / acceleration;
        float timeRemain = iniTime;

        while (timeRemain >= 0f)
        {
            speed = iniSpeed + acceleration * (iniTime - timeRemain);

            timeRemain -= Time.deltaTime;
            yield return null;
        }
        speed = 0f;
        Debug.Log(distanceDone);
        Debug.Log(distance);

        StopCoroutine(SlowStop(distance));
    }

    private void Update()
    {
        if (consideredAsMoving)
            distanceDone += Time.deltaTime * speed;
    }

    public void SetConsideredMoving(bool toggle) => consideredAsMoving = toggle;

}
