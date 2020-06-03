using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour, IActivatable
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
        Activate();
    }

    public void SetCursor(Temporality temporality)
    {
        if (temporality == Temporality.Modern)
            Cursor.SetCursor(modernCursorTexture, hotSpot, cursorMode);
        else if (temporality == Temporality.Past)
            Cursor.SetCursor(pastCursorTexture, hotSpot, cursorMode);
    }

    public void Activate()
    {
        Cursor.visible = true;
    }

    public void Desactivate()
    {
        Cursor.visible = false;
    }
}
