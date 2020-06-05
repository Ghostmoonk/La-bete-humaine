using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void PlaySound(string soundName)
    {
        SoundManager.Instance.PlaySound(source, soundName);
    }

    public void PlaySound(AudioClip soundClip)
    {
        SoundManager.Instance.PlaySound(source, soundClip);
    }
}
