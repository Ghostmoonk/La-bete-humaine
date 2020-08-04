using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShaderValuesModifier : MonoBehaviour
{
    [SerializeField] protected FloatShaderValue[] startShaderValues;
    [SerializeField] protected Vector4ShaderValue[] startShaderVector4Values;
    [SerializeField] protected float endValue;
    [SerializeField] protected float duration;
    protected string currentValueName;
    [SerializeField] protected UnityEvent TransitionEnd;

    protected Tween tween;

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
    public void SetCurrentValueName(string newValueName)
    {
        currentValueName = newValueName;
    }

    public void SetEndValue(float value)
    {
        endValue = value;
    }
}

[System.Serializable]
public struct FloatShaderValue
{
    public string shaderName;
    public float startValue;
    public float endValue;
}

[System.Serializable]
public struct Vector4ShaderValue
{
    public string shaderName;
    public Vector4 startValue;
    public Vector4 endValue;
}