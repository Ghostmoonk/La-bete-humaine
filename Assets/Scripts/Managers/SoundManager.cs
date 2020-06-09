using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }
    AudioSource defaultSource;
    [SerializeField] List<Sound> soundsList;
    Dictionary<string, AudioClip[]> soundsDico;

    List<AudioSource> toRestarList;

    [Range(1, 10)]
    [SerializeField] int pauseVolReduction;
    [SerializeField] float durationPauseDbReduction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        soundsDico = new Dictionary<string, AudioClip[]>();
        for (int i = 0; i < soundsList.Count; i++)
        {
            soundsDico.Add(soundsList[i].name, soundsList[i].clips);
        }
        if (GameObject.FindGameObjectWithTag("DefaultSource") != null)
            defaultSource = GameObject.FindGameObjectWithTag("DefaultSource").GetComponent<AudioSource>();
    }

    public void PlaySound(AudioSource source, string soundName)
    {
        AudioClip[] clips;
        if (soundsDico.TryGetValue(soundName, out clips))
        {
            source.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            source.Play();
        }
        else
            throw new Exception("Le son au nom " + soundName + " n'existe pas dans le manager");
    }

    public void PlaySound(AudioSource source, AudioClip soundClip)
    {
        source.clip = soundClip;
        source.Play();
    }

    public void PlaySound(string soundName)
    {
        if (defaultSource == null)
            throw new MissingReferenceException("Il manque la source par défaut");

        AudioClip[] clips;
        if (soundsDico.TryGetValue(soundName, out clips))
        {
            defaultSource.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            defaultSource.Play();
        }
        else
            throw new Exception("Le son au nom " + soundName + " n'existe pas dans le manager");
    }

    public void ChangeMixerVolume(AudioMixer mixer, float newVolume, float duration)
    {
        float volume = 0f;

        if (mixer.GetFloat("Volume", out volume))
        {
            DOTween.To(() => volume, x => volume = x, newVolume, duration);
        }
    }

    public void FadeOutAllSource(float duration)
    {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source.isPlaying)
            {
                float currentVolume = source.volume;
                Tweener tween = source.DOFade(0f, duration);
                tween.OnComplete(() => { source.Pause(); source.volume = currentVolume; toRestarList.Add(source); });
            }
        }
    }

    public void FadeInAllSource(float duration)
    {
        foreach (AudioSource source in toRestarList)
        {
            float currentVolume = source.volume;
            source.volume = 0f;
            source.UnPause();
            Tweener tween = source.DOFade(currentVolume, duration);
            tween.OnComplete(() => { source.Stop(); source.volume = currentVolume; });
        }
    }

    public void PauseDecreaseVolume(AudioMixerGroup mixer)
    {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source.outputAudioMixerGroup == mixer)
            {
                source.DOFade(source.volume / pauseVolReduction, durationPauseDbReduction).SetUpdate(true);
            }
        }
    }

    public void PauseIncreaseVolume(AudioMixerGroup mixer)
    {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source.outputAudioMixerGroup == mixer)
            {
                source.DOFade(source.volume * pauseVolReduction, durationPauseDbReduction).SetUpdate(true);
            }
        }
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
    }
}
