using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ThinkBubble : MonoBehaviour
{
    Animator animator;
    Image contentImg;

    void Start()
    {
        animator = GetComponent<Animator>();
        contentImg = transform.GetChild(0).GetComponent<Image>();

        gameObject.SetActive(false);
    }

    public void Activate(Sprite sprite)
    {
        contentImg.sprite = sprite;
        gameObject.SetActive(true);
    }

    public void FadeHide()
    {
        animator.SetBool("Show", false);
    }

    public void Desactivate()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }

}
