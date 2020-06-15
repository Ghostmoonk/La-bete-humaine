﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;
    public bool loopDifferentSounds = false;
    [Range(0f, 2f)]
    [HideInInspector] public float delayBetweenSounds;
    private string soundNameToPlay;

    public void PlaySound(string soundName)
    {
        SoundManager.Instance.PlaySound(source, soundName);
    }

    public void PlaySound(AudioClip soundClip)
    {
        SoundManager.Instance.PlaySound(source, soundClip);
    }

    public void PlayLoopSound(string soundName)
    {
        loopDifferentSounds = true;
        soundNameToPlay = soundName;
        StartCoroutine(PlayDifferentSoundsLoop());
    }

    public void SetDelayBetweenSounds(float newDelay)
    {
        delayBetweenSounds = newDelay;
    }

    public void ChangeSoundNameToPlayLoop(string soundName)
    {
        soundNameToPlay = soundName;
    }

    IEnumerator PlayDifferentSoundsLoop()
    {
        do
        {
            PlaySound(soundNameToPlay);
            yield return new WaitForSeconds(source.clip.length + delayBetweenSounds);
        }
        while (loopDifferentSounds);
    }
}