using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleMover : MonoBehaviour
{
    Tweener tween;
    [SerializeField] float transitionDuration;
    bool toggler;
    Vector2 initialAnchoredPos;

    [SerializeField] UnityEvent StartToggleOn;
    [SerializeField] UnityEvent StartToggleOff;

    private void Start()
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
            tween = GetComponent<RectTransform>().DOAnchorPosY(initialAnchoredPos.y + rectT.sizeDelta.y, transitionDuration);
            StartToggleOn?.Invoke();
        }
        else
        {
            tween = GetComponent<RectTransform>().DOAnchorPosY(GetComponent<RectTransform>().anchoredPosition.y - rectT.sizeDelta.y, transitionDuration);
            StartToggleOff?.Invoke();
        }

        toggler = !toggler;

    }
}
