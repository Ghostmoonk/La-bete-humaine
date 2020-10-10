using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TMP_Dropdown))]
public class LevelSelector : MonoBehaviour
{
    int selectedLevel = 1;

    void Start()
    {
        var optionDataList = new List<TMP_Dropdown.OptionData>();

        for (int i = 2; i <= SceneManager.sceneCountInBuildSettings; ++i)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i - 1));
            optionDataList.Add(new TMP_Dropdown.OptionData(name));
            Debug.Log(name);
        }

        GetComponent<TMP_Dropdown>().ClearOptions();
        GetComponent<TMP_Dropdown>().AddOptions(optionDataList);
    }

    public void SetSelectedLevel(int value)
    {
        selectedLevel = value + 1;
    }

    public void AsynchronLoadSelectedLevel()
    {
        ScenesManager.Instance.LoadSceneAsync(selectedLevel);
    }
}
