using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class BasicSlider : MonoBehaviour
{
    [System.Serializable]
    public class ReferenceContent
    {
        public RectTransform refTransform;
        public ReferenceImage refData;
    }
    public SliderSubject sliderSubject = new SliderSubject();

    [SerializeField] ReferenceContent[] contentToSlide;
    ReferenceContent currentContent;
    [Max(1)]
    [SerializeField] Vector2 direction;
    Vector2 sliderLength;

    [SerializeField] float duration;
    [SerializeField] float transitionTime;
    [SerializeField] Ease ease;
    bool shouldSlide;

    private void Start()
    {
        RefImageChanger[] refImgChanger = FindObjectsOfType<RefImageChanger>();
        for (int i = 0; i < refImgChanger.Length; i++)
        {
            sliderSubject.AddObserver(refImgChanger[i].observer);
        }

        currentContent = contentToSlide[0];
        sliderSubject.Complete(currentContent.refData);
        shouldSlide = true;
        sliderLength = Vector2.zero;

        for (int i = 0; i < contentToSlide.Length; i++)
        {
            sliderLength += new Vector2(contentToSlide[i].refTransform.rect.width, contentToSlide[i].refTransform.rect.height);
            contentToSlide[i].refTransform.GetComponent<Image>().sprite = contentToSlide[i].refData.sprite;
        }

        StartCoroutine(SlideCo());
    }

    public void Slide()
    {
        for (int i = 0; i < contentToSlide.Length; i++)
        {
            Tweener tween = contentToSlide[i].refTransform.DOAnchorPos(new Vector2(contentToSlide[i].refTransform.anchoredPosition.x + contentToSlide[i].refTransform.rect.width * direction.x, contentToSlide[i].refTransform.anchoredPosition.y + contentToSlide[i].refTransform.rect.height * direction.y), transitionTime).SetEase(ease);
            if (contentToSlide[i] == currentContent)
            {
                int index = 0;
                if (i + 1 < contentToSlide.Length)
                    index = i + 1;

                ReferenceContent previousContent = currentContent;
                tween.OnStart(() => { SetCurrentContent(contentToSlide[index]); sliderSubject.Complete(contentToSlide[index].refData); });

                tween.OnComplete(() => { TeleportBack(previousContent.refTransform); });
            }
        }
    }

    public void ToggleSliding()
    {
        shouldSlide = !shouldSlide;
    }

    private IEnumerator SlideCo()
    {
        float timer = 0f;

        while (true)
        {
            if (shouldSlide)
            {
                timer += Time.deltaTime;
                yield return null;

                if (timer >= duration)
                {
                    Slide();
                    timer = 0f;
                }
            }
            else
                yield return null;
        }
    }

    private void TeleportBack(RectTransform rectT) => rectT.anchoredPosition -= sliderLength * direction;

    private void SetCurrentContent(ReferenceContent refC)
    {
        currentContent = refC;
    }

}
