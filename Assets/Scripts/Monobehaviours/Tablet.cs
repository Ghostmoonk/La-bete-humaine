using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Tablet : MonoBehaviour
{
    Animator animator;
    Coroutine displayer;
    #region UI

    RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI contentText;
    #endregion
    bool active;

    private void OnEnable()
    {
        active = false;
    }

    public void ToggleVisibility()
    {
        animator.SetBool("Active", !animator.GetBool("Active"));
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        displayer = StartCoroutine(DisplayText(FetchText(0)));
    }

    IEnumerator DisplayText(string text)
    {
        yield return null;
        contentText.text = text;
    }

    public string FetchText(int id)
    {
        return TextsLoader.Instance.textsDico[id].content;
    }
}

//var builder = new System.Text.StringBuilder(text.Length * 26);
//Color32 color = contentText.color;