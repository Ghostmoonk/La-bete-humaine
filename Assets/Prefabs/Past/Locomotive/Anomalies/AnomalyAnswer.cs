﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalyAnswer : MonoBehaviour
{
    [SerializeField] Image resultIcon;
    [SerializeField] Toggle toggle;
    [SerializeField] string toggleOnSoundName;
    [SerializeField] string toggleOffSoundName;

    public void UpdateSprite(Sprite newSprite, Color color)
    {
        resultIcon.sprite = newSprite;
        resultIcon.color = color;
    }

    public void UpdateSprite(Sprite newSprite) => resultIcon.sprite = newSprite;

    public Toggle GetToggle() => toggle;

    public Image GetResultIconImage() => resultIcon;

    public void PlayToggleSound(bool toggle)
    {
        if (toggle)
            SoundManager.Instance.PlaySound(GetComponentInParent<AudioSource>(), toggleOnSoundName);
        else
            SoundManager.Instance.PlaySound(GetComponentInParent<AudioSource>(), toggleOffSoundName);

    }
}