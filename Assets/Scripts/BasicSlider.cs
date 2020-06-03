using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicSlider : MonoBehaviour
{
    [SerializeField] RectTransform[] contentToSlide;
    [SerializeField] RectTransform currentContent;
    [Range(0, 1)]
    [SerializeField] Vector2 direction;
    Vector2 sliderLength;

    private void Start()
    {
        sliderLength = Vector2.zero;

        for (int i = 0; i < contentToSlide.Length; i++)
        {
            sliderLength += contentToSlide[i].anchoredPosition;
        }
    }

    public void Slide(float duration)
    {
        for (int i = 0; i < contentToSlide.Length; i++)
        {
            Tweener tween = contentToSlide[i].DOAnchorPos(new Vector2(currentContent.sizeDelta.x * direction.x, currentContent.sizeDelta.y * direction.y), duration);
            if (contentToSlide[i] == currentContent)
            {
                if (i + 1 < contentToSlide.Length)
                    tween.OnComplete(() => { TeleportBack(currentContent); SetCurrentContent(contentToSlide[i + 1]); });
                else
                    tween.OnComplete(() => { TeleportBack(currentContent); SetCurrentContent(contentToSlide[0]); });

            }
        }
    }

    private void TeleportBack(RectTransform rectT) => rectT.anchoredPosition -= sliderLength;

    private void SetCurrentContent(RectTransform rectT) => currentContent = rectT;

}
