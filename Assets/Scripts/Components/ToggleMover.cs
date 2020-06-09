using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleMover : MonoBehaviour, IIndependantTween
{
    Tweener tween;
    [SerializeField] bool isUnityTimeScaleInDependant;
    [SerializeField] float transitionDuration;
    bool toggler;
    Vector2 initialAnchoredPos;

    [SerializeField] UnityEvent StartToggleOn;
    [SerializeField] UnityEvent EndToggleOn;

    [SerializeField] UnityEvent StartToggleOff;
    [SerializeField] UnityEvent EndToggleOff;

    private void OnEnable()
    {
        initialAnchoredPos = GetComponent<RectTransform>().anchoredPosition;

    }

    public void MoveBySizeY(RectTransform rectT)
    {
        if (tween != null)
            if (tween.IsPlaying())
                tween.Pause();

        if (!toggler)
        {
            tween = GetComponent<RectTransform>().DOAnchorPosY(initialAnchoredPos.y + rectT.rect.height, transitionDuration).SetUpdate(isUnityTimeScaleInDependant);
            StartToggleOn?.Invoke();
            tween.OnComplete(() => EndToggleOn.Invoke());
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosY(GetComponent<RectTransform>().anchoredPosition.y - rectT.rect.height, transitionDuration).SetUpdate(isUnityTimeScaleInDependant);
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
        Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        if (!toggler)
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(initialAnchoredPos.x + rectT.rect.width, transitionDuration).SetUpdate(isUnityTimeScaleInDependant);
            StartToggleOn?.Invoke();
            tween.OnComplete(() => EndToggleOn.Invoke());
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosX(GetComponent<RectTransform>().anchoredPosition.x - rectT.rect.width, transitionDuration).SetUpdate(isUnityTimeScaleInDependant);
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
