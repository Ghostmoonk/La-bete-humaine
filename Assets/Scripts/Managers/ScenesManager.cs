using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    #region Singleton
    private static ScenesManager _instance;
    public static ScenesManager Instance
    {
        get
        {
            return _instance;
        }
    }
    AsyncOperation asyncLoad;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] Temporality temporality;

    private void Start()
    {
        CursorManager.Instance.SetCursor(temporality);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        CursorManager.Instance.SetCursor(temporality);
        CursorManager.Instance.Activate();
    }

    public void ActivateAsyncScene()
    {
        asyncLoad.allowSceneActivation = true;
        CursorManager.Instance.SetCursor(temporality);
        CursorManager.Instance.Activate();
    }

    public void LoadSceneAsync(int index)
    {
        asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;
    }

    public void SwitchTemporality(int newTemporality)
    {
        temporality = (Temporality)newTemporality;
    }

    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public Temporality GetTemporality()
    {
        return temporality;
    }

}

public enum Temporality { Modern = 0, Past = 1 }
