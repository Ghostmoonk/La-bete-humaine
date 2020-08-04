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

    public void Rotate(float duration)
    {
        Tween tween = transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + additionnalAngleDeg), duration).SetEase(ease).SetUpdate(false);
        tween.OnComplete(() => { OnRotationEnds?.Invoke(); ResetAngle(); });
    }

    public void SetNewAngle(float angle)
    {
        additionnalAngleDeg = angle;
    }

    private void ResetAngle()
    {
        if (transform.eulerAngles.z > 360 || transform.eulerAngles.z < -360)
        {
            transform.eulerAngles =
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 360 * Mathf.FloorToInt(transform.eulerAngles.z / 360));
        }
    }
}
