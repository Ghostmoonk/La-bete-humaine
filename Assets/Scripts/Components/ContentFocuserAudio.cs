using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFocuserAudio : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] string focusOutSoundName;
    [SerializeField] string focusInSoundName;

    private void Start()
    {
        GetComponent<IFocusable>().OnFocusIn += delegate { SoundManager.Instance.PlaySound(source, focusInSoundName); };
        GetComponent<IFocusable>().OnFocusOut += delegate { SoundManager.Instance.PlaySound(source, focusOutSoundName); };
    }
}
