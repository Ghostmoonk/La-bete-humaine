using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThinkBubble : MonoBehaviour
{
    Animator animator;
    Renderer renderer;
    Image contentImg;

    void Start()
    {
        animator = GetComponent<Animator>();
        contentImg = transform.GetChild(0).GetComponent<Image>();

        gameObject.SetActive(false);
    }

    public void Activate(Texture2D sprite)
    {
        //contentImg.sprite = sprite;
        renderer.material.SetTexture("_MainTexture", sprite);
        gameObject.SetActive(true);
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
        gameObject.SetActive(false);
    }

}
