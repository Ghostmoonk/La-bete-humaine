using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Handle the anchored movement oof the attached object depending on the size of a RectTransform
public class ToggleMover : MonoBehaviour, IIndependantTween
{
    Tweener tween;
    [SerializeField] bool isUnityTimeScaleInDependant;
    [SerializeField] float transitionDuration;
    bool toggler = false;
    Vector2 initialAnchoredPos;
    [Range(-1, 1)]
    [SerializeField] int direction = 1;
    [SerializeField] Ease ease = Ease.Linear;
    public UnityEvent StartToggleOn;
    public UnityEvent EndToggleOn;

    public UnityEvent StartToggleOff;
    public UnityEvent EndToggleOff;

    private void Awake()
    {
        ResetInitialAnchorPos();
    }

    public bool GetToggler() => toggler;

    public void ResetInitialAnchorPos() => initialAnchoredPos = GetComponent<RectTransform>().anchoredPosition;

    public void MoveBySizeY(RectTransform rectT)
    {
        if (tween != null)
            if (tween.IsPlaying())
                tween.Pause();

        if (!toggler)
        {
            tween = GetComponent<RectTransform>().DOAnchorPosY(initialAnchoredPos.y + rectT.rect.height * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            StartToggleOn?.Invoke();
            tween.OnComplete(() => EndToggleOn.Invoke());
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosY(GetComponent<RectTransform>().anchoredPosition.y - rectT.rect.height * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            StartToggleOff?.Invoke();
            tween.OnComplete(() => EndToggleOff.Invoke());
        }

        toggler = !toggler;

    }
    public void MoveBySizeX(float size)
    {
        if (tween != null)
            if (tween.IsPlaying())
                tween.Pause();

        if (!toggler)
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(initialAnchoredPos.x + size * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            Debug.Log(size * direction);
            StartToggleOn?.Invoke();
            tween.OnComplete(() => EndToggleOn.Invoke());
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(GetComponent<RectTransform>().anchoredPosition.x - size * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            StartToggleOff?.Invoke();
            tween.OnComplete(() => EndToggleOff.Invoke());
        }
        toggler = !toggler;
    }

    public void MoveBySizeX(RectTransform rectT)
    {
        if (tween != null)
            if (tween.IsPlaying())
                tween.Pause();

        if (!toggler)
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(initialAnchoredPos.x + rectT.rect.width * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            StartToggleOn?.Invoke();
            tween.OnComplete(() => EndToggleOn.Invoke());
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(GetComponent<RectTransform>().anchoredPosition.x - rectT.rect.width * direction, transitionDuration).SetUpdate(isUnityTimeScaleInDependant).SetEase(ease);
            StartToggleOff?.Invoke();
            tween.OnComplete(() => EndToggleOff.Invoke());
        }
        toggler = !toggler;

    }

    public void SetIndependantTween(bool IsIndependant)
    {
        isUnityTimeScaleInDependant = IsIndependant;
    }
}

public interface IIndependantTween
{
    void SetIndependantTween(bool IsIndependant);
}
