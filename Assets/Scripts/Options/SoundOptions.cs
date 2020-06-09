using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider ambianceSlider;
    [SerializeField] Slider interfaceSlider;
    [SerializeField] Slider mainSlider;

    [SerializeField] AudioMixer mixer;

    [SerializeField] float minDecibel;
    [SerializeField] float maxDecibel;

    private void Start()
    {
        mainSlider.value = Mathf.InverseLerp(minDecibel, maxDecibel, GetExposedParameterValue("MasterVol"));

        musicSlider.value = Mathf.InverseLerp(minDecibel, maxDecibel, GetExposedParameterValue("MusicVol"));

        ambianceSlider.value = Mathf.InverseLerp(minDecibel, maxDecibel, GetExposedParameterValue("AmbianceVol"));

        interfaceSlider.value = Mathf.InverseLerp(minDecibel, maxDecibel, GetExposedParameterValue("InterfaceVol"));
    }

    private float GetExposedParameterValue(string name)
    {
        mixer.GetFloat(name, out float value);
        return value;
    }

    #region UpdateVolumes

    public void UpdateMasterVolume(float value)
    {
        float newVol = Mathf.Lerp(minDecibel, maxDecibel, value);
        mixer.SetFloat("MasterVol", newVol);
    }

    public void UpdateMusicVolume(float value)
    {
        float newVol = Mathf.Lerp(minDecibel, maxDecibel, value);
        mixer.SetFloat("MusicVol", newVol);
    }

    public void UpdateAmbianceVolume(float value)
    {
        float newVol = Mathf.Lerp(minDecibel, maxDecibel, value);
        mixer.SetFloat("AmbianceVol", newVol);
    }

    public void UpdateInterfaceVolume(float value)
    {
        float newVol = Mathf.Lerp(minDecibel, maxDecibel, value);
        mixer.SetFloat("InterfaceVol", newVol);
    }

    public void UpdateEnvironmentVolume(float value)
    {
        float newVol = Mathf.Lerp(minDecibel, maxDecibel, value);
        mixer.SetFloat("InterfaceVol", newVol);
    }

    #endregion
}
