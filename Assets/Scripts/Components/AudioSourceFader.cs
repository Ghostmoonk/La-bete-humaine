using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceFader : MonoBehaviour
{
    AudioSource source;
    [Range(0, 1)]
    [SerializeField] float fadeVolume = 0;

    [SerializeField] UnityEvent EndFadeVolume;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void FadeVolume(float duration)
    {
        Tween tween = source.DOFade(fadeVolume, duration);
        tween.OnComplete(() => { EndFadeVolume?.Invoke(); });
    }

    public void SetFadeVolume(float newFadeVolume) => fadeVolume = newFadeVolume;
}
