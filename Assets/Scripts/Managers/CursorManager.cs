using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    private static CursorManager instance;
    public static CursorManager Instance
    {
        get
        {
            return instance;
        }
    }
    public Texture2D modernCursorTexture;
    public Texture2D pastCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [SerializeField] AudioSource cursorSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        if (ScenesManager.Instance.GetTemporality() == Temporality.Modern)
            Cursor.SetCursor(modernCursorTexture, hotSpot, cursorMode);
        else if (ScenesManager.Instance.GetTemporality() == Temporality.Past)
            Cursor.SetCursor(pastCursorTexture, hotSpot, cursorMode);
    }

}
