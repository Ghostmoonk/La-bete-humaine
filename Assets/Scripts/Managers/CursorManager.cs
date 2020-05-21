using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

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
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
