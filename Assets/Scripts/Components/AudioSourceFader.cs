using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceFader : MonoBehaviour
{
    AudioSource source;
    [Range(0, 1)]
    [SerializeField] float fadeVolume = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void FadeVolume(float duration)
    {
        source.DOFade(fadeVolume, duration);
    }

    public void SetFadeVolume(float newFadeVolume) => fadeVolume = newFadeVolume;
}
