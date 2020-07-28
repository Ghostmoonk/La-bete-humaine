using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlinkerUI : MonoBehaviour
{
    [SerializeField] GraphicColor[] graphics;
    Dictionary<GraphicColor, Color> graphicIniColorDico;
    [SerializeField] float period;
    [SerializeField] float periodVariance;
    [SerializeField] Ease ease;

    [SerializeField] bool canBlink = true;

    private void Start()
    {
        graphicIniColorDico = new Dictionary<GraphicColor, Color>();

        foreach (GraphicColor item in graphics)
        {
            graphicIniColorDico.Add(item, item.graphic.color);
        }
    }
    public void StartBlink()
    {
        foreach (var item in graphicIniColorDico)
        {
            Blink(item);
        }
    }

    private void Blink(KeyValuePair<GraphicColor, Color> pair)
    {
        float blinkTime = Random.Range(period - periodVariance, period + periodVariance);
        Tween tween = pair.Key.graphic.DOColor(pair.Key.color, blinkTime).SetEase(ease);

        tween.OnComplete(() => BlinkBack(pair));

    }

    private void BlinkBack(KeyValuePair<GraphicColor, Color> pair)
    {
        float blinkTime = Random.Range(period - periodVariance, period + periodVariance);
        Tween tween = pair.Key.graphic.DOColor(pair.Value, blinkTime).SetEase(ease);

        if (canBlink)
        {
            tween.OnComplete(() => Blink(pair));
        }
    }

    public void ToggleBlink(bool doBlink)
    {
        canBlink = doBlink;
        Debug.Log(canBlink);
    }
}

[System.Serializable]
public struct GraphicColor
{
    public Graphic graphic;
    public Color color;
}
