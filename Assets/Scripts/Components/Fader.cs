using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public float defaultFadeOutDuration;
    public float fullOpacityTime;
    public Ease fadeInEase;
    public Ease fadeOutEase;
    public Color color;

    [Header("Events")]
    public UnityEvent EndFadeInEvents;
    public UnityEvent TransitionFadeEvents;
    public UnityEvent EndTransitionFadeEvents;
    public UnityEvent EndFadeOutEvents;

}
