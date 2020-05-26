using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using TextContent;
using DG.Tweening;

public class TMP_WordHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    HighlightNotifier notifier;
    [SerializeField] private TextMeshProUGUI textComponent;

    private int currentSelectedWord = -1;

    [SerializeField] private Color32 notHighlightedColor;
    [SerializeField] private Color32 highlightedColor;

    private void OnEnable()
    {
        if (textComponent == null)
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();

    }
    private void Start()
    {
        notifier = new HighlightNotifier();
        notifier.AddObserver(GlossaryDisplayer.Instance.glossaryObserver);
    }

    public void HighlighGlossaryWords(float duration)
    {
        Debug.Log(textComponent.textInfo.wordCount);
        for (int i = 0; i < textComponent.textInfo.wordCount; i++)
        {
            if (TextsLoader.Instance.ContainWordInGlossary(textComponent.textInfo.wordInfo[i].GetWord()) != -1)
            {
                StartCoroutine(SmoothChangeColor(textComponent.textInfo.wordInfo[i], duration));
                //ChangeWordColor(textComponent.textInfo.wordInfo[i], notHighlightedColor);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    /**
    *@param wordInfo - contains the specific word informations
    *@param color - The word will get this color
    **/
    private void ChangeWordColor(TMP_WordInfo wordInfo, Color32 color)
    {
        for (int i = 0; i < wordInfo.characterCount; ++i)
        {
            int charIndex = wordInfo.firstCharacterIndex + i;
            int meshIndex = textComponent.textInfo.characterInfo[charIndex].materialReferenceIndex;
            int vertexIndex = textComponent.textInfo.characterInfo[charIndex].vertexIndex;

            Color32[] vertexColors = textComponent.textInfo.meshInfo[meshIndex].colors32;

            //We loop 4 times because tehre are 4 corners on the character vertice
            for (int j = 0; j < 4; j++)
            {
                vertexColors[vertexIndex + j] = color;
            }
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    IEnumerator SmoothChangeColor(TMP_WordInfo wordInfo, float duration)
    {
        Color32 finalColor = notHighlightedColor;
        Color32 baseColor = textComponent.color;
        DOTween.To(() => baseColor, x => baseColor = x, notHighlightedColor, duration);

        while (!baseColor.Equals(finalColor))
        {
            ChangeWordColor(wordInfo, baseColor);
            yield return null;
        }
        StopCoroutine(SmoothChangeColor(wordInfo, duration));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, Input.mousePosition, Camera.main);

        if (wordIndex != -1)
        {
            TMP_WordInfo wInfo;

            //If this is already the selected word
            if (wordIndex == currentSelectedWord)
            {
                wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
                notifier.BroadcastHighlight(Input.mousePosition, null);
                ChangeWordColor(wInfo, notHighlightedColor);
                currentSelectedWord = -1;
                return;
            }
            // if there is a selected word but not the current one
            else if (currentSelectedWord != -1)
            {
                wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
                ChangeWordColor(wInfo, notHighlightedColor);
            }

            wInfo = textComponent.textInfo.wordInfo[wordIndex];
            //We fetch in the glossary if the highlighted word is in
            int wordId = TextsLoader.Instance.ContainWordInGlossary(wInfo.GetWord());

            //S'il est != de -1, il y est
            if (wordId != -1)
            {
                currentSelectedWord = wordIndex;
                ChangeWordColor(wInfo, highlightedColor);
            }
            else
            {
                return;
            }
            Vector3 wordCoordinates = textComponent.textInfo.characterInfo[textComponent.textInfo.wordInfo[wordIndex].firstCharacterIndex].bottomLeft;
            notifier.BroadcastHighlight(wordCoordinates, TextsLoader.Instance.glossaryDico[wordId]);
        }
        //on a cliquer ailleurs, on retire la couleur et on notifie l'observer
        else
        {
            if (currentSelectedWord != -1)
            {
                ChangeWordColor(textComponent.textInfo.wordInfo[currentSelectedWord], highlightedColor);
                notifier.BroadcastHighlight(Input.mousePosition, null);
                currentSelectedWord = -1;
            }
        }

    }

    //On exit, automatically reset the currentSelectedWord if there is one
    public void OnPointerExit(PointerEventData eventData)
    {
        //if (currentSelectedWord != -1)
        //{
        //    TMP_WordInfo wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
        //    ChangeWordColor(wInfo, notHighlightedColor);
        //    currentSelectedWord = -1;
        //    notifier.BroadcastHighlight(Vector3.zero);
        //}
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
