using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rotator2D : MonoBehaviour
{
    [SerializeField] float additionnalAngleDeg;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent OnRotationEnds;
    [SerializeField] Transform[] transformsToRotate;
    Tween tween;

    public void Rotate(float duration)
    {
        foreach (var item in transformsToRotate)
        {
            tween = item.DORotate(new Vector3(0, 0, item.eulerAngles.z + additionnalAngleDeg), duration).SetEase(ease).SetUpdate(false);

        }
        tween.OnComplete(() => { OnRotationEnds?.Invoke(); ResetAngle(); });
    }

    public void SetNewAngle(float angle)
    {
        additionnalAngleDeg = angle;
    }

    private void ResetAngle()
    {
        foreach (var item in transformsToRotate)
        {
            if (item.eulerAngles.z > 360 || item.eulerAngles.z < -360)
            {
                item.eulerAngles =
                    new Vector3(item.eulerAngles.x, item.eulerAngles.y, item.eulerAngles.z - 360 * Mathf.FloorToInt(item.eulerAngles.z / 360));
            }
        }

    }

    public void RotateFromValue(float value)
    {
        foreach (var item in transformsToRotate)
        {
            item.transform.eulerAngles = new Vector3(item.eulerAngles.x, item.eulerAngles.y, item.eulerAngles.z + additionnalAngleDeg * value / 10);
        }
        ResetAngle();
    }
}
