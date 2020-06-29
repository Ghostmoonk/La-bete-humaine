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
    [SerializeField] Texture2D modernCursorTexture;
    [SerializeField] Texture2D pastCursorTexture;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] Vector2 hotSpot = Vector2.zero;
    Vector3 screenPoint;
    [SerializeField] AudioListener cursorListener;
    float cursorListenerZ;

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
        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Activate();
    }

    private void Start()
    {
        if (cursorListener != null)
        {
            cursorListenerZ = Camera.main.WorldToScreenPoint(transform.position).z;
            screenPoint = Camera.main.WorldToScreenPoint(cursorListener.transform.position);
        }
    }
    private void Update()
    {
        if (cursorListener != null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

            cursorListener.transform.position = curPosition;
        }
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
        Cursor.lockState = CursorLockMode.None;
    }

    public void Desactivate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
