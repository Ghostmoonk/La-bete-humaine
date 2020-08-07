using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightTweener : MonoBehaviour
{
    Light2D light2D;
    [SerializeField] float transitionTime;
    [SerializeField] Color endColor;
    [SerializeField] Ease transitionEase;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    public void SetTransitionTime(float newTime) => transitionTime = newTime;

    public void TweenColor(float duration)
    {
        StartCoroutine(SmoothChangeColor(duration));
    }
    public void Tweenintensity(float newValue)
    {
        Debug.Log("Allé zinedine");
        StartCoroutine(SmoothChangeIntensity(newValue));
    }

    IEnumerator SmoothChangeColor(float duration)
    {
        Color startColor = light2D.color;
        Color changingColor = light2D.color;

        DOTween.To(() => changingColor, x => changingColor = x, endColor, duration).SetEase(transitionEase);

        while (light2D.color != endColor)
        {
            light2D.color = changingColor;
            yield return new WaitForSeconds(Time.deltaTime);

        }

        endColor = startColor;
    }
    IEnumerator SmoothChangeIntensity(float newValue)
    {
        float changingIntensity = light2D.intensity;

        DOTween.To(() => changingIntensity, x => changingIntensity = x, newValue, transitionTime).SetEase(transitionEase);

        while (light2D.volumeOpacity != newValue)
        {
            light2D.intensity = changingIntensity;
            yield return new WaitForSeconds(Time.deltaTime);

        }
    }


}
