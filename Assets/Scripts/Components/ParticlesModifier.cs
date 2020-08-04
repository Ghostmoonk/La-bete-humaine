using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticlesModifier : MonoBehaviour
{
    [SerializeField] ParticleSystem[] pSystems;
    Dictionary<ParticleSystem, ParticlesInitialData> pSystemData;

    private void Start()
    {
        pSystemData = new Dictionary<ParticleSystem, ParticlesInitialData>();
        foreach (ParticleSystem pSystem in pSystems)
        {
            pSystemData.Add(pSystem, new ParticlesInitialData
            {
                baseScalarVelocityX = pSystem.velocityOverLifetime.xMultiplier
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
}

public struct ParticlesInitialData
{
    public float baseScalarVelocityX;
}
