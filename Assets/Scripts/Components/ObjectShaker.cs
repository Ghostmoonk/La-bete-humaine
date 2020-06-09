using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaker : Shaker, IIndependantTween
{
    bool shaking;

    [SerializeField] float periodDuration;
    [SerializeField] float periodDurationVariance;

    [SerializeField] float amplitude;
    [SerializeField] float amplitudeVariance;

    [SerializeField] bool playOnStart;

    Tween tweenShake;
    [SerializeField] bool isUnityTimeScaleInDependant;
    [SerializeField] bool resetPosAtEnd;

    float deltaTime;

    private void Start()
    {
        if (playOnStart)
            ContinuousShake();

        deltaTime = Time.deltaTime;

    }
    public override void ContinuousShake()
    {
        shaking = true;
        Debug.Log(Time.timeScale);
        Debug.Log(DOTween.timeScale);
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
        if (!resetPosAtEnd)
            tweenShake.Kill();

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
            tweenShake = transform.DOMove(newRandomPos, durationTime).SetEase(Ease.Flash).SetUpdate(isUnityTimeScaleInDependant);

            float timer = 0f;
            while (timer < durationTime)
            {
                yield return new WaitForSeconds(deltaTime);
                timer += deltaTime;

                if (!shaking)
                    break;
            }
            //yield return new WaitForSeconds(durationTime);
            initialPos = transform.position - randomTarget;
        }
        if (resetPosAtEnd)
            tweenShake = transform.DOMove(initialPos, periodDuration).SetEase(Ease.Flash).SetUpdate(isUnityTimeScaleInDependant); ;
    }

    public void SetIndependantTween(bool IsIndependant)
    {
        isUnityTimeScaleInDependant = IsIndependant;
    }
}
