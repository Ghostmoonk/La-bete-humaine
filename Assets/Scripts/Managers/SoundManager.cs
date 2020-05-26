using System;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] List<Sound> soundsList;
    Dictionary<string, AudioClip[]> soundsDico;

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

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
    }
}
