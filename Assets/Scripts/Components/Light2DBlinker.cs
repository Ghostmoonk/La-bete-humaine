using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Light2DBlinker : MonoBehaviour, IBlinker
{
    [SerializeField] Light2D[] lights;
    [Min(0)]
    [SerializeField] float period;
    [Min(0)]
    [SerializeField] float periodVariance;
    [SerializeField] Vector2 minMaxIntensity;

    bool blink;

    public void StartBlink()
    {
        blink = true;
        foreach (Light2D light in lights)
        {
            StartCoroutine(Blink(light));
        }
    }

    public void StoptBlink()
    {
        blink = false;
        foreach (Light2D light in lights)
        {
            StartCoroutine(SmoothSwitchOff(light));
        }
    }

    IEnumerator SmoothSwitchOff(Light2D light)
    {
        float time = Random.Range(period - periodVariance, period + periodVariance);
        float remainingtime = 0f;
        float currentIntensity = light.intensity;
        while (remainingtime < time)
        {
            remainingtime -= Time.deltaTime;
            light.intensity = Mathf.Lerp(currentIntensity, 0f, remainingtime / time);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Blink(Light2D light)
    {
        while (blink)
        {
            float totalBlinkTime = Random.Range(period - periodVariance, period + periodVariance);
            float blinkTime = 0f;
            float newIntensity = Random.Range(minMaxIntensity.x, minMaxIntensity.y);
            float currentIntensity = light.intensity;

            while (blinkTime < totalBlinkTime)
            {
                light.intensity = Mathf.Lerp(currentIntensity, newIntensity, blinkTime / totalBlinkTime);
                blinkTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}

public interface IBlinker
{
    void StartBlink();
    void StoptBlink();
}
