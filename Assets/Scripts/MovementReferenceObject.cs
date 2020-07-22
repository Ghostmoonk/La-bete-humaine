using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementReferenceObject : MonoBehaviour
{
    public float speed;
    [HideInInspector] public float distanceDone;
    bool consideredAsMoving = true;

    public void Stop(float stopDuration)
    {
        DOTween.To(() => speed, x => speed = x, 0f, stopDuration);
    }

    public void ChangeSpeed(float newSpeed, float duration)
    {
        DOTween.To(() => speed, x => speed = x, newSpeed, duration);
    }

    private void Update()
    {
        if (consideredAsMoving)
            distanceDone += Time.deltaTime * speed;
    }

    public void SetConsideredMoving(bool toggle) => consideredAsMoving = toggle;

}
