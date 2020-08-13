using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementReferenceObject : MonoBehaviour
{

    public float speed;
    [Tooltip("A reference speed, more like the 'normal speed'")]
    [SerializeField] float mediumSpeed;
    [SerializeField] float timeToChangeSpeed;
    [SerializeField] Ease accelerationCurve;
    [HideInInspector] public float distanceDone;
    bool consideredAsMoving = true;
    [SerializeField] FloatUnityEvent OnSpeedChange;
    [SerializeField] FloatUnityEvent OnSpeedChangeConstant;
    bool isUpdatingSpeed = false;
    Tween tween;

    public void StopInTime(float stopDuration)
    {
        ChangeSpeed(0, stopDuration);
    }

    public void ChangeSpeed(float newSpeed)
    {
        isUpdatingSpeed = true;
        if (tween != null)
            if (tween.IsPlaying())
                tween.Kill();
        tween = DOTween.To(() => speed, x => speed = x, newSpeed, timeToChangeSpeed).SetEase(accelerationCurve);
        tween.OnComplete(() => { isUpdatingSpeed = false; });
    }

    public void ChangeSpeed(float newSpeed, float specificDuration)
    {
        isUpdatingSpeed = true;
        if (tween != null)
            if (tween.IsPlaying())
                tween.Kill();
        tween = DOTween.To(() => speed, x => speed = x, newSpeed, specificDuration).SetEase(accelerationCurve);
        tween.OnComplete(() => { isUpdatingSpeed = false; });
    }

    public void ChangeSpeedDelayed(float delay) => Invoke(nameof(SetMediumSpeed), delay);

    public void SetMediumSpeed() => ChangeSpeed(mediumSpeed);

    public void SetMediumSpeed(float newMediumSpeed) => mediumSpeed = newMediumSpeed;

    public void SetTimeToChangeSpeed(float newAccelerationTime) => timeToChangeSpeed = newAccelerationTime;


    public void SlowStopToDistance(float ratioDist)
    {
        Debug.Log("Debut slow, distance parcourue :" + distanceDone);
        Debug.Log("Equivalent ratio : " + Mathf.Lerp(0, LocomotiveRouteManager.Instance.GetCurrentStationInRoute().distwithNextStation, ratioDist));

        float realDist = ratioDist * LocomotiveRouteManager.Instance.GetCurrentStationInRoute().distwithNextStation;
        StartCoroutine(SlowStop(realDist));

    }

    IEnumerator SlowStop(float distance)
    {
        float iniSpeed = speed;

        float acceleration = -Mathf.Pow(iniSpeed, 2) / (2 * distance);
        float iniTime = -iniSpeed / acceleration;
        float timeRemain = iniTime;

        isUpdatingSpeed = true;
        while (timeRemain >= 0f)
        {
            speed = iniSpeed + acceleration * (iniTime - timeRemain);

            timeRemain -= Time.deltaTime;
            yield return null;
        }
        speed = 0f;
        isUpdatingSpeed = false;
        StopCoroutine(SlowStop(distance));
    }

    private void Update()
    {
        if (consideredAsMoving)
            distanceDone += Time.deltaTime * speed;
        if (isUpdatingSpeed)
            OnSpeedChange?.Invoke(speed);
        OnSpeedChangeConstant?.Invoke(speed);
    }

    public void SetConsideredMoving(bool toggle) => consideredAsMoving = toggle;

}

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float>
{

}
