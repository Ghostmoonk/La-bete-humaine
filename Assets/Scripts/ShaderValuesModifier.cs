using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShaderValuesModifier : MonoBehaviour
{
    Graphic rend;
    [SerializeField] FloatShaderValue[] startShaderValues;
    [SerializeField] float endValue;
    [SerializeField] float duration;

    public UnityEvent TransitionEnd;
    Tween tween;

    private void Awake()
    {
        if (GetComponent<Image>() == null)
            throw new Exception("Il manque un composant Graphic");
        else
            rend = GetComponent<Image>();

        rend.material = new Material(rend.material);

        for (int i = 0; i < startShaderValues.Length; i++)
            rend.material.SetFloat(startShaderValues[i].shaderName, startShaderValues[i].value);

    }

    public void ChangeFloatValue(string valueName)
    {
        tween = rend.material.DOFloat(endValue, valueName, duration);
        tween.OnComplete(() => { TransitionEnd.Invoke(); });
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void SetEndValue(float value)
    {
        this.endValue = value;
    }

    public void SetMaterial(Material material)
    {
        rend.material = new Material(material);
    }

}
[Serializable]
public struct FloatShaderValue
{
    public string shaderName;
    public float value;
}
