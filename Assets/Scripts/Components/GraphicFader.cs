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
    [SerializeField] Ease ease;
    [SerializeField] Graphic[] graphic;

    public UnityEvent EndFadeOutText;
    public UnityEvent StartFadeIn;
    public UnityEvent EndFadeInText;


    public void FadeOut(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            graphic[i].DOKill();
            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, 0f), duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
        }
    }

    public void FadeIn(float duration)
    {
        StartFadeIn?.Invoke();
        for (int i = 0; i < graphic.Length; i++)
        {
            graphic[i].color = new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, 0f);
            graphic[i].DOKill();

            Tweener tween = graphic[i].DOColor(new Color(graphic[i].color.r, graphic[i].color.g, graphic[i].color.b, maxOpacity), duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeInText?.Invoke(); });
        }
    }

    public void FadeOutByParenting(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                Tweener tween = item.DOColor(new Color(item.color.r, item.color.g, item.color.b, 0f), duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
                if (i == graphic.Length - 1)
                    tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
            }
        }
    }

    public void FadeInByParenting(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);
                Tweener tween = item.DOColor(new Color(item.color.r, item.color.g, item.color.b, maxOpacity), duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
                if (i == graphic.Length - 1)
                    tween.OnComplete(() => { EndFadeInText?.Invoke(); });
            }

        }
    }


    public void SetIndependantTween(bool IsIndependant)
    {
        IsUnityTimeScaleIndependant = IsIndependant;
    }
}
