using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ManuscriptToggler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    Sprite spriteRef;
    [HideInInspector] public static List<ManuscriptToggler> togglers = new List<ManuscriptToggler>();
    ImageSlider imageSlider;
    bool toggle;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //Cette condition est plus logique à placer dans le textHolder, car là il y a des refs inutiles
        if (GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath != null)
        {
            imageSlider = GameObject.FindGameObjectWithTag("ManuscriptContainer").GetComponent<ImageSlider>();
            toggle = false;
            spriteRef = Resources.Load<Sprite>("images/" + GetComponentInParent<TextHolder>().simpleText.textData.manuscritPath.Split('.')[0]);
        }
        else
            gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        togglers.Add(this);
    }

    private void OnDisable()
    {
        togglers.Remove(this);
    }

    public void SetToggle(bool toggle)
    {
        this.toggle = toggle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < togglers.Count; i++)
        {
            if (togglers[i] != this)
                togglers[i].SetToggle(false);
        }

        toggle = !toggle;

        //Cache
        if (!toggle)
            imageSlider.SlideIn();

        //Affiche
        else
        {
            imageSlider.ShowImage(spriteRef, transform.position.y, GetComponentInParent<TextHolder>().simpleText.textData.manuscritSource);

            if (animator.GetBool("Active"))
                animator.SetBool("Active", false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!toggle)
            animator.SetBool("Active", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Active", false);
    }
}
