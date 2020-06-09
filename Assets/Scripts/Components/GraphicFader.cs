using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GraphicFader : MonoBehaviour, IIndependantTween
{
    [SerializeField] float maxOpacity = 1;
    [SerializeField] bool IsUnityTimeScaleIndependant;
    [SerializeField] Graphic[] graphic;

    [SerializeField] UnityEvent EndFadeOutText;
    [SerializeField] UnityEvent EndFadeInText;


    public void FadeOut(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, 0f), duration).SetUpdate(IsUnityTimeScaleIndependant);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
        }
    }

    public void FadeIn(float duration)
    {
        Debug.Log("fadein");
        for (int i = 0; i < graphic.Length; i++)
        {
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, maxOpacity), duration).SetUpdate(IsUnityTimeScaleIndependant);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeInText?.Invoke(); });
        }
    }

    public void SetIndependantTween(bool IsIndependant)
    {
        IsUnityTimeScaleIndependant = IsIndependant;
    }
}
