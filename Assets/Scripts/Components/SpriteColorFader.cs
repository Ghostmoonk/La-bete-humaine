using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteColorFader : MonoBehaviour, IIndependantTween, IFade
{
    [SerializeField] SpriteRenderer[] spritesR;
    [SerializeField] Color fadeOutColor;
    [SerializeField] Color fadeInColor;
    [SerializeField] Ease ease;
    [SerializeField] bool IsUnityTimeScaleIndependant;

    public UnityEvent EndFadeOutText;
    public UnityEvent StartFadeIn;
    public UnityEvent EndFadeInText;

    public void SetIndependantTween(bool IsIndependant)
    {
        IsUnityTimeScaleIndependant = IsIndependant;
    }

    public void FadeOut(float duration)
    {
        for (int i = 0; i < spritesR.Length; i++)
        {
            spritesR[i].DOKill();
            Tweener tween = spritesR[i].DOColor(fadeOutColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

            if (i == spritesR.Length - 1)
                tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
        }
    }

    public void FadeInByParenting(float duration)
    {
        StartFadeIn?.Invoke();
        for (int i = 0; i < spritesR.Length; i++)
        {
            foreach (SpriteRenderer item in spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>())
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);
                Tweener tween = item.DOColor(fadeInColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
                if (i == spritesR.Length - 1 && item == spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>()[spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>().Length - 1])
                    tween.OnComplete(() => { EndFadeInText?.Invoke(); });
            }

        }
    }

    public void FadeOutByParenting(float duration)
    {
        for (int i = 0; i < spritesR.Length; i++)
        {
            foreach (SpriteRenderer item in spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>())
            {
                Tweener tween = item.DOColor(fadeOutColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);

                if (i == spritesR.Length - 1 && item == spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>()[spritesR[i].transform.GetComponentsInChildren<SpriteRenderer>().Length - 1])
                    tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
            }
        }
    }

    public void FadeIn(float duration)
    {
        Debug.Log("fadein");
        StartFadeIn?.Invoke();
        for (int i = 0; i < spritesR.Length; i++)
        {
            spritesR[i].color = new Color(spritesR[i].color.r, spritesR[i].color.g, spritesR[i].color.b, 0f);
            spritesR[i].DOKill();

            Tweener tween = spritesR[i].DOColor(fadeInColor, duration).SetUpdate(IsUnityTimeScaleIndependant).SetEase(ease);
            if (i == spritesR.Length - 1)
                tween.OnComplete(() => { EndFadeInText?.Invoke(); });
        }
    }

}
