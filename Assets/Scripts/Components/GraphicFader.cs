using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GraphicFader : MonoBehaviour
{
    [SerializeField] Graphic graphic;


    [SerializeField] UnityEvent EndFadeOutText;
    [SerializeField] UnityEvent EndFadeInText;

    public void FadeOut(float duration)
    {
        Debug.Log("fadeout");
        Tweener tween = graphic.DOColor(new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0f), duration);
        tween.OnComplete(() => { EndFadeOutText?.Invoke(); });
    }

    public void FadeIn(float duration)
    {
        Tweener tween = graphic.DOColor(new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1f), duration);
        tween.OnComplete(() => { EndFadeInText?.Invoke(); });
    }

}
