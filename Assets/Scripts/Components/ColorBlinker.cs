using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ColorBlinker : MonoBehaviour
{
    SpriteRenderer spriteR;

    [SerializeField] float period;
    [SerializeField] float periodVariance;
    [SerializeField] Color color;
    [SerializeField] Ease ease;

    bool canBlink = true;

    private void Awake()
    {
        if (!GetComponent<SpriteRenderer>())
            throw new MissingComponentException("Il manque un composant Image ou un composant SpriteRenderer sur l'object :" + gameObject);
    }

    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void StartBlink()
    {
        Color colorToGo = color;
        color = spriteR.color;

        float blinkTime = Random.Range(period - periodVariance, period + periodVariance);
        Tweener tween = spriteR.DOColor(colorToGo, blinkTime).SetEase(ease);

        if (canBlink)
            tween.OnComplete(() => StartBlink());
        else
        {
            tween = spriteR.DOColor(new Color(colorToGo.r, colorToGo.g, colorToGo.b, 0f), blinkTime).SetEase(ease);
        }
    }

    public void StopBlink()
    {
        canBlink = false;
    }
}
