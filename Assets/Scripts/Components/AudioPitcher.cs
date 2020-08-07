using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioPitcher : MonoBehaviour
{
    [Tooltip("For this value, pitch will be 1, then it's proportional to it")]
    [SerializeField] float baseValue;
    [SerializeField] AudioSource[] sources;
    [SerializeField] AudioMixerGroup mixer;
    [SerializeField] string exposedValueName;

    public void Repitch(float refValue)
    {
        refValue = Mathf.Abs(refValue);
        mixer.audioMixer.SetFloat(exposedValueName, 1 / (refValue / baseValue));
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].pitch = refValue / baseValue;
        }
    }
}
