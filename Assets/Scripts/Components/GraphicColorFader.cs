using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GraphicColorFader : MonoBehaviour, IIndependantTween, IFade
{
    [SerializeField] Color fadeInColor;
    [SerializeField] Color fadeOutColor;
    [SerializeField] bool IsUnityTimeScaleIndependant;
    [SerializeField] Ease ease;
    [SerializeField] Graphic[] graphic;

    public UnityEvent EndFadeOut;
    public UnityEvent StartFadeIn;
    public UnityEvent EndFadeIn;


    public void FadeOut(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            graphic[i].DOKill();
            Tweener tween = graphic[i].DOColor(fadeOutColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeOut?.Invoke(); });
        }
    }

    public void FadeIn(float duration)
    {
        StartFadeIn?.Invoke();
        for (int i = 0; i < graphic.Length; i++)
        {
            //graphic[i].color = fadeOutColor;
            graphic[i].DOKill();

            Tweener tween = graphic[i].DOColor(fadeInColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
            if (i == graphic.Length - 1)
                tween.OnComplete(() => { EndFadeIn?.Invoke(); });
        }
    }

    public void FadeOutByParenting(float duration)
    {
        for (int i = 0; i < graphic.Length; i++)
        {
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                Tweener tween = item.DOColor(new Color(item.color.r, item.color.g, item.color.b, 0f), duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

                if (i == graphic.Length - 1 && item == graphic[i].transform.GetComponentsInChildren<Graphic>()[graphic[i].transform.GetComponentsInChildren<Graphic>().Length - 1])
                    tween.OnComplete(() => { EndFadeOut?.Invoke(); });
            }
        }
    }

    public void FadeInByParenting(float duration)
    {
        StartFadeIn?.Invoke();
        for (int i = 0; i < graphic.Length; i++)
        {
            foreach (Graphic item in graphic[i].transform.GetComponentsInChildren<Graphic>())
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);
                Tweener tween = item.DOColor(fadeInColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
                if (i == graphic.Length - 1 && item == graphic[i].transform.GetComponentsInChildren<Graphic>()[graphic[i].transform.GetComponentsInChildren<Graphic>().Length - 1])
                    tween.OnComplete(() => { EndFadeIn?.Invoke(); });
            }

        }
    }


    public void SetIndependantTween(bool IsIndependant)
    {
        IsUnityTimeScaleIndependant = IsIndependant;
    }
}
