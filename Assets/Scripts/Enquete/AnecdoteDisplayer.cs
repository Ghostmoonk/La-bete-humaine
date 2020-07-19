using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnecdoteDisplayer : MonoBehaviour, IActivatable
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI textContent;
    [SerializeField] Image image;
    [Header("Navigation")]
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    AnecdoteData currentAnecdoteData;
    int currentIndex = 0;

    public void Activate()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
        {
            Desactivate();
        }
    }

    public void Desactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetContent(AnecdoteData anecdoteData)
    {
        currentAnecdoteData = anecdoteData;
        currentIndex = 0;

        DisplayContent();

        if (anecdoteData.anecdoteContents.Length > 1)
        {
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            previousButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }
    }

    private void DisplayContent()
    {
        title.text = currentAnecdoteData.anecdoteContents[currentIndex].title;

        if (currentAnecdoteData.anecdoteContents[currentIndex].text.Length > 0)
        {
            textContent.text = currentAnecdoteData.anecdoteContents[currentIndex].text;
            textContent.gameObject.SetActive(true);
        }
        else
            textContent.gameObject.SetActive(false);

        if (currentAnecdoteData.anecdoteContents[currentIndex].sprite != null)
        {
            image.sprite = currentAnecdoteData.anecdoteContents[currentIndex].sprite;
            image.gameObject.SetActive(true);
        }
        else
            image.gameObject.SetActive(false);
    }

    public void GoNextCurrentAnecdoteContent(int index)
    {
        currentIndex += index;

        DisplayContent();

    }
}
