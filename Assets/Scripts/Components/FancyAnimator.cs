using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyAnimator : MonoBehaviour
{
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    public void Jiggle(float strength) => transform.DOShakeRotation(duration, new Vector3(0, 0, strength)).SetEase(ease);

    public void ReScale(float value) => transform.DOScale(new Vector3(value, value, transform.localScale.z), duration).SetEase(ease);

    public void SetDuration(float amount) => duration = amount;

    public void SetEase(Ease newEase) => ease = newEase;
}
