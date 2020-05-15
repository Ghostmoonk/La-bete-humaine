using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using TextContent;

public class TMP_WordHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    HighlightNotifier notifier;
    [SerializeField] private TextMeshProUGUI textComponent;

    private int currentSelectedWord = -1;

    private Color32 normalColor;
    [SerializeField] private Color32 highlightedColor;

    private void OnEnable()
    {
        if (textComponent == null)
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        normalColor = textComponent.color;

    }
    private void Start()
    {
        notifier = new HighlightNotifier();
        notifier.AddObserver(GlossaryDisplayer.Instance.glossaryObserver);
        Debug.Log(GlossaryDisplayer.Instance.glossaryObserver + " added");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    //@params
    //wordInfo - contains the specific word informations
    //color - The word will get this color
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
                ChangeWordColor(wInfo, normalColor);
                currentSelectedWord = -1;
                return;
            }
            // if there is a selected word but not the current one
            else if (currentSelectedWord != -1)
            {
                wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
                ChangeWordColor(wInfo, normalColor);
            }

            currentSelectedWord = wordIndex;

            wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
            int wordId = TextsLoader.Instance.ContainWordInGlossary(wInfo.GetWord());

            //We fetch in the glossary if the highlighted word is in

            //S'il est != de -1, il y est
            if (wordId != -1)
            {
                ChangeWordColor(wInfo, highlightedColor);
            }
            else
            {
                return;
            }

            notifier.BroadcastHighlight(Input.mousePosition, TextsLoader.Instance.glossaryDico[wordId]);
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
        if (currentSelectedWord != -1)
        {
            TMP_WordInfo wInfo = textComponent.textInfo.wordInfo[currentSelectedWord];
            ChangeWordColor(wInfo, normalColor);
            currentSelectedWord = -1;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
