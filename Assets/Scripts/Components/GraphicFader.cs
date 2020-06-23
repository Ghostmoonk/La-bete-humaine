using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Fade in or out Graphics like text or image
public class GraphicFader : MonoBehaviour, IIndependantTween
{
    [SerializeField] float maxOpacity = 1;
    [SerializeField] bool IsUnityTimeScaleIndependant;
    [SerializeField] Graphic[] graphic;

    public UnityEvent EndFadeOutText;
    public UnityEvent EndFadeInText;


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
        for (int i = 0; i < graphic.Length; i++)
        {
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, maxOpacity), duration).SetUpdate(IsUnityTimeScaleIndependant);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeInText?.Invoke(); });
        }
    }

    public void FadeOutByParenting(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, 0f), duration).SetUpdate(IsUnityTimeScaleIndependant);
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                item.DOColor(new Color(item.color.r, item.color.g, item.color.b, 0f), duration);
            }
            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
        }
    }

    public void FadeInByParenting(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, maxOpacity), duration).SetUpdate(IsUnityTimeScaleIndependant);
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                item.DOColor(new Color(item.color.r, item.color.g, item.color.b, maxOpacity), duration);
            }

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeInText?.Invoke(); });
        }
    }


    public void SetIndependantTween(bool IsIndependant)
    {
        IsUnityTimeScaleIndependant = IsIndependant;
    }
}
