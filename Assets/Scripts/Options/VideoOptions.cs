using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown screenModeDropdown;
    [SerializeField] FullScreenMode currentFullScreenMode;

    private void Start()
    {
        screenModeDropdown.onValueChanged.AddListener(delegate { SetFullScreen(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { ChangeResolution(); });
    }
    public void ChangeResolution(int resolutionId)
    {
        string[] resolutionString = resolutionDropdown.options[resolutionId].text.Split(new char[] { 'x' });
        Screen.SetResolution(int.Parse(resolutionString[0]), int.Parse(resolutionString[1]), currentFullScreenMode);

    }
    public void ChangeResolution()
    {
        string[] resolutionString = resolutionDropdown.options[resolutionDropdown.value].text.Split(new char[] { 'x' });
        Screen.SetResolution(int.Parse(resolutionString[0]), int.Parse(resolutionString[1]), currentFullScreenMode);

        Debug.Log(int.Parse(resolutionString[0]) + " x " + int.Parse(resolutionString[1]));
        Debug.Log((int)currentFullScreenMode + " - " + currentFullScreenMode);

    }

    private void SetAvailableResolutions()
    {
        resolutionDropdown.ClearOptions();
        List<string> resolutionStrings = new List<string>();
        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionStrings.Add(resolution.width + " x " + resolution.height);
        }
        resolutionDropdown.AddOptions(resolutionStrings);
    }

    public void SetFullScreen(FullScreenMode screenMode)
    {
        currentFullScreenMode = screenMode;
        Screen.fullScreenMode = currentFullScreenMode;
    }

    public void SetFullScreen(int screenMode)
    {
        currentFullScreenMode = (FullScreenMode)screenMode;
        Screen.fullScreenMode = currentFullScreenMode;
    }

    private void SetFullScreen()
    {
        currentFullScreenMode = (FullScreenMode)screenModeDropdown.value;
        Screen.fullScreenMode = currentFullScreenMode;
    }
}
