using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaderManager : MonoBehaviour
{
    private static FaderManager instance;
    public static FaderManager Instance
    {
        get
        {
            return instance;
        }
    }

    Animator fadeAnimator;

    public delegate void FadeOutDelegate();
    public delegate void FadeInDelegate();

    public FadeOutDelegate fadeOutDelegate;
    public FadeInDelegate fadeInDelegate;

    [SerializeField] Image faderImg;
    [SerializeField] Fader fader;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(this);

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fadeAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        FadeIn(0f);
        faderImg.color = fader.color;
        FadeOut(fader.defaultFadeOutDuration);
    }

    public void Fade() => fadeAnimator.SetTrigger("Fade");


    public void LongFade(float fadeTime)
    {
        Fade();
        fadeAnimator.SetBool("AutoFadeIn", false);
        Invoke(nameof(ResetAutoFadeIn), fadeTime);
    }

    private void ResetAutoFadeIn() => fadeAnimator.SetBool("AutoFadeIn", true);

    //Vers l'opaque
    public void FadeIn(float duration)
    {
        Tweener fadeInTween = faderImg.DOFade(1, duration).SetEase(fader.fadeInEase);
        fadeInTween.OnComplete(() => StartCoroutine(HoldFade(fadeInTween)));
    }

    //Réalise les actions au milieu du temps de fade en full opacité
    IEnumerator HoldFade(Tweener fadeTween)
    {
        fadeTween.Kill();
        FadeInActions();
        yield return new WaitForSeconds(fader.fullOpacityTime / 2);
        fader?.TransitionFadeEvents.Invoke();
        yield return new WaitForSeconds(fader.fullOpacityTime / 2);
        fader.EndTransitionFadeEvents?.Invoke();
        //Debug.Log(fader?.EndTransitionFadeEvents.GetPersistentEventCount());
        StopCoroutine(HoldFade(fadeTween));
    }

    //Vers la transparence
    public void FadeOut(float duration)
    {
        Tweener fadeOutween = faderImg.DOFade(0, duration).SetEase(fader.fadeOutEase);
        fadeOutween.OnComplete(() => FadeOutActions());
    }

    //public void SetColor(Color col) => faderImg.color = col;

    public void FadeOutActions()
    {
        if (fadeOutDelegate != null)
        {
            fadeOutDelegate();
            fadeOutDelegate = null;
        }
        fader.EndFadeOutEvents?.Invoke();
    }

    public void FadeInActions()
    {
        if (fadeInDelegate != null)
        {
            fadeInDelegate();
            fadeInDelegate = null;
        }
        fader.EndFadeInEvents.Invoke();
    }

    public void SetFader(Fader fader)
    {
        this.fader = fader;
        faderImg.color = new Color(fader.color.r, fader.color.g, fader.color.b, faderImg.color.a);
    }
}
