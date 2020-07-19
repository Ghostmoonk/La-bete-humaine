using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ContentFocuser : MonoBehaviour, IFocusable
{
    Transform initialParent;
    int initialChildIndex;
    [SerializeField] float duration;
    Vector3 initialScale;

    bool focused = false;
    [Range(1f, 2f)]
    [SerializeField] float sizeMultiplier;
    [SerializeField] RectTransform parentToResize;

    GraphicFader focusedContentBackground;

    public event Action OnFocusIn;
    public event Action OnFocusOut;

    void Start()
    {
        initialParent = transform.parent;
        initialScale = transform.localScale;

        initialChildIndex = transform.GetSiblingIndex();

        focusedContentBackground = GameObject.FindGameObjectWithTag("FocusedContentBackground").GetComponent<GraphicFader>();
    }

    public void Focus()
    {
        if (!focused)
            FocusIn();
        else
            FocusOut();
    }

    public void FocusIn()
    {
        OnFocusIn();
        transform.DOKill();
        transform.SetParent(GameObject.FindGameObjectWithTag("FrontCanvas").transform, true);
        transform.DOMove(GameObject.FindGameObjectWithTag("ScreenCenter").transform.position, duration);
        transform.DOScale(initialScale * sizeMultiplier, duration);
        focused = true;

        focusedContentBackground.FadeIn(duration);
    }

    public void FocusOut()
    {
        OnFocusOut();
        transform.DOKill();
        transform.SetParent(initialParent, true);
        transform.SetSiblingIndex(initialChildIndex);

        transform.localScale = initialScale;
        Resizer.ResizeLayout(transform.root.GetComponent<RectTransform>());

        focusedContentBackground.FadeOut(0f);
        focused = false;
    }
}

