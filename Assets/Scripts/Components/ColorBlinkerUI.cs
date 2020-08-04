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
    [SerializeField] Tween[] tweens;

    [SerializeField] bool canBlink = true;

    private void Start()
    {
        graphicIniColorDico = new Dictionary<GraphicColor, Color>();

        foreach (GraphicColor item in graphics)
        {
            graphicIniColorDico.Add(item, item.graphic.color);
        }
        tweens = new Tween[graphics.Length];
    }
    public void StartBlink()
    {
        int compteur = 0;
        foreach (var item in graphicIniColorDico)
        {
            Blink(item, compteur);
            compteur++;
        }
    }

    private void Blink(KeyValuePair<GraphicColor, Color> pair, int compteur)
    {
        float blinkTime = Random.Range(period - periodVariance, period + periodVariance);
        tweens[compteur] = pair.Key.graphic.DOColor(pair.Key.color, blinkTime).SetEase(ease);

        tweens[compteur].OnComplete(() => BlinkBack(pair, compteur));

    }

    private void BlinkBack(KeyValuePair<GraphicColor, Color> pair, int compteur)
    {
        float blinkTime = Random.Range(period - periodVariance, period + periodVariance);
        tweens[compteur] = pair.Key.graphic.DOColor(pair.Value, blinkTime).SetEase(ease);

        if (canBlink)
        {
            tweens[compteur].OnComplete(() => Blink(pair, compteur));
        }
    }

    public void CancelBlink()
    {
        if (canBlink)
            foreach (var item in graphicIniColorDico)
            {
                item.Key.graphic.color = new Color(item.Value.r, item.Value.g, item.Value.b, item.Key.graphic.color.a);
            }
    }

    public void ToggleBlink(bool doBlink)
    {
        canBlink = doBlink;
    }

    public void KillTweens()
    {
        foreach (var item in tweens)
        {
            item.Pause();
            item.Kill();
        }
    }

}

[System.Serializable]
public struct GraphicColor
{
    public Graphic graphic;
    public Color color;
}
