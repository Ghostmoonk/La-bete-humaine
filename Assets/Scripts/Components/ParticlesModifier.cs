using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticlesModifier : MonoBehaviour
{
    [SerializeField] ParticleSystem[] pSystems;
    Dictionary<ParticleSystem, ParticlesInitialData> pSystemData;
    [SerializeField] MinMaxGradient toggledColor;


    private void Start()
    {
        pSystemData = new Dictionary<ParticleSystem, ParticlesInitialData>();
        foreach (ParticleSystem pSystem in pSystems)
        {
            pSystemData.Add(pSystem, new ParticlesInitialData
            {
                baseScalarVelocityX = pSystem.velocityOverLifetime.xMultiplier,
                toggledColor = toggledColor
            });
        }
    }

    public void AdaptVelocityX(float value)
    {
        foreach (KeyValuePair<ParticleSystem, ParticlesInitialData> item in pSystemData)
        {
            VelocityOverLifetimeModule velocityOverLifetime = item.Key.velocityOverLifetime;
            velocityOverLifetime.xMultiplier = item.Value.baseScalarVelocityX + item.Value.baseScalarVelocityX * value;
        }
    }

    public void ToggleParticlesColorDelayed(float delay)
    {
        StartCoroutine(ToggleParticlesColor(delay));
    }

    IEnumerator ToggleParticlesColor(float delay)
    {
        yield return new WaitForSeconds(delay);
        Dictionary<ParticleSystem, ParticlesInitialData> switchDico = new Dictionary<ParticleSystem, ParticlesInitialData>();
        foreach (KeyValuePair<ParticleSystem, ParticlesInitialData> item in pSystemData)
        {
            MainModule particlesModule = item.Key.main;

            MinMaxGradient swapColor = item.Key.main.startColor;
            particlesModule.startColor = item.Value.toggledColor;

            switchDico.Add(item.Key, new ParticlesInitialData { baseScalarVelocityX = item.Value.baseScalarVelocityX, toggledColor = swapColor });
        }
        foreach (var item in switchDico)
        {
            pSystemData[item.Key] = item.Value;
        }
    }

    public void SetParticlesDuration(float duration)
    {
        for (int i = 0; i < pSystems.Length; i++)
        {
            MainModule particleModule = pSystems[i].main;
            particleModule.duration = duration;
        }
    }

    public void ContinuousPlay(float time)
    {
        foreach (ParticleSystem item in pSystems)
        {
            StartCoroutine(PlayLoop(item, time));
        }
    }

    IEnumerator PlayLoop(ParticleSystem pSystem, float time)
    {
        float remainingTime = time;
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(pSystem.main.duration / 2);
            pSystem.Play();
            yield return new WaitForSeconds(pSystem.main.duration / 2);
            pSystem.Play();
            remainingTime -= pSystem.main.duration;
        }
    }
}

public struct ParticlesInitialData
{
    public float baseScalarVelocityX;
    public MinMaxGradient toggledColor;
}
