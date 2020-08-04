using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FogShaderValuesModifierSprite : ShaderValuesModifier
{
    Renderer rend;

    Vector4 iniSize;
    Vector4 iniDirSpeed;

    private void Awake()
    {
        if (GetComponent<Renderer>() == null)
            throw new Exception("Il manque un composant Renderer");
        else
            rend = GetComponent<Renderer>();

        rend.material = new Material(rend.material);

        for (int i = 0; i < startShaderValues.Length; i++)
            rend.material.SetFloat(startShaderValues[i].shaderName, startShaderValues[i].startValue);

        iniSize = rend.material.GetVector("_Size");
        iniDirSpeed = rend.material.GetVector("_DirectionSpeed");

    }

    public void ChangeFloatValue(string valueName)
    {
        tween = rend.material.DOFloat(endValue, valueName, duration);
        tween.OnComplete(() => { TransitionEnd.Invoke(); });
    }

    public void SetMaterial(Material material)
    {
        rend.material = new Material(material);
    }

    public void ChangeFloatValue(string valueName, float newValue, float duration = 0f)
    {
        tween = rend.material.DOFloat(newValue, valueName, duration);

        if (duration > 0F)
            tween.OnComplete(() => { TransitionEnd.Invoke(); });
    }
    public void ChangeFloatValue(float newValue, float duration = 0f)
    {
        tween = rend.material.DOFloat(newValue, currentValueName, duration);

        if (duration > 0F)
            tween.OnComplete(() => { TransitionEnd.Invoke(); });
    }

    public void ChangeDirectionSpeedX(float speed, float duration = 0f)
    {
        float iniDirSpeedX = 0f;
        for (int i = 0; i < startShaderValues.Length; i++)
        {
            if (startShaderValues[i].shaderName == "_DirectionSpeed")
            {
                iniDirSpeedX = startShaderValues[i].startValue;
            }
        }

        tween = rend.material.DOVector(new Vector4(iniDirSpeedX + iniDirSpeedX * speed, 0f, 0f), currentValueName, duration);

        if (duration > 0f)
            tween.OnComplete(() => { TransitionEnd.Invoke(); });
    }

    public void ChangeSizeX(float value)
    {
        rend.material.SetVector("_Size", new Vector4((value + iniSize.x) / (value + 1), rend.material.GetVector("_Size").y, rend.material.GetVector("_Size").z, rend.material.GetVector("_Size").w));
    }
}