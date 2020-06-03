using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : Shaker
{

    [SerializeField] int vibrato;
    [SerializeField] float shakeStrength;

    public override void Shake(float duration)
    {
        Camera.main.DOShakePosition(duration, shakeStrength, vibrato);
    }

    public override void ContinuousShake()
    {
        Camera.main.DOShakePosition(300000f, shakeStrength, vibrato);
    }

    public void SetVibrato(int _vibrato)
    {
        vibrato = _vibrato;
    }

    public void SetStrength(float _strength)
    {
        shakeStrength = _strength;
    }

    public override void StopShake()
    {
        Camera.main.DOKill();
    }
}
