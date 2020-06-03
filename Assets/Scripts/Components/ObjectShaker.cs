using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaker : Shaker
{
    bool shaking;

    [SerializeField] float periodDuration;
    [SerializeField] float periodDurationVariance;

    [SerializeField] float amplitude;
    [SerializeField] float amplitudeVariance;

    public override void ContinuousShake()
    {
        shaking = true;
        StartCoroutine(ShakeCo());
    }

    public override void Shake(float duration)
    {
        shaking = true;
        StartCoroutine(ShakeCo());
        Invoke(nameof(StopShake), duration);
    }

    public override void StopShake()
    {
        shaking = false;
        StopCoroutine(ShakeCo());
    }

    IEnumerator ShakeCo()
    {
        Vector3 initialPos = transform.position;
        while (shaking)
        {
            Vector3 randomTarget = Random.insideUnitCircle * Random.Range(amplitude - amplitudeVariance, amplitude + amplitudeVariance);

            Vector3 newRandomPos = randomTarget + initialPos;

            float durationTime = Random.Range(periodDuration - periodDurationVariance, periodDuration + periodDurationVariance);
            transform.DOMove(newRandomPos, durationTime).SetEase(Ease.Flash);
            yield return new WaitForSeconds(durationTime);
        }
        transform.DOMove(initialPos, periodDuration).SetEase(Ease.Flash);
        transform.DOKill();
    }
}
