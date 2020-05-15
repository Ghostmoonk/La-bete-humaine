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

    private void OnEnable()
    {
    }
    public void Activate(Sprite sprite)
    {
        contentImg.sprite = sprite;
        gameObject.SetActive(true);
        transform.DOShakePosition(5f);
    }

    //IEnumerator SwapSprite(Sprite[] sprites)
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < sprites.Length; i++)
    //        {
    //            contentImg.sprite = sprites[i];
    //            yield return new WaitForSeconds(3f);
    //        }
    //    }
    //}

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
