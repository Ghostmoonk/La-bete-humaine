using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;
    public static DialogManager Instance
    {
        get
        {
            return instance;
        }
    }

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
        dialogBox.SetActive(false);
    }

    [Header("Prefabs & Container")]
    public GameObject dialogBox;
    public Transform answersContainer;
    public GameObject answerPrefab;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI sentenceText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image skipImage;
    [SerializeField] Image talkingCharacterImage;

    [Header("Texts")]
    [SerializeField] float answersFadeDuration = 1f;
    Dialog currentDialog;

    //public delegate void VoidDelegate();
    //public VoidDelegate OnDialogOver;

    [Header("Characters")]
    [SerializeField] Character[] characters;
    [SerializeField] float showSkipTimerLimit;
    [SerializeField] bool shouldShowCharactersSprite = true;
    Dictionary<string, Sprite> charactersSprites = new Dictionary<string, Sprite>();

    [Header("Events")]
    public DialogEvents[] dialogsEvents;

    [Range(0.1f, 20f)]
    [SerializeField] float sentenceTypingSpeed;

    bool typingSentence = false;
    float showSkipTimer;

    public Dialog GetCurrentDialog()
    {
        return currentDialog;
    }

    public void ResetCurrentDialog()
    {
        currentDialog.Reset();
    }

    private void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            charactersSprites.Add(characters[i].name, characters[i].dialogSprite);
        }
    }

    public void StartDialog()
    {
        if (!dialogBox.activeSelf)
            dialogBox.SetActive(true);

        int currentDialogId = DialogsLoader.Instance.dialogsDico.FirstOrDefault(x => x.Value == currentDialog).Key;
        DialogEvents? dialogEvent = FindCorrespondingEvents(currentDialogId);
        if (dialogEvent != null)
        {
            dialogEvent.Value.StartEvents?.Invoke();
        }

        DisplayNextSentence();
    }

    public void ToggleDialogBoxVisibility(bool active)
    {
        dialogBox.GetComponent<Animator>().SetBool("Active", active);
        if (!active)
        {
            StopDialog();
            dialogBox.GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            dialogBox.GetComponent<Image>().raycastTarget = true;
        }
    }

    public void FinishSentence()
    {
        if (typingSentence)
        {
            typingSentence = false;
        }
        else
        {
            DisplayNextSentence();
        }
    }

    private void DisplayNextSentence()
    {
        ToggleSkip(false);

        if (currentDialog == null)
            return;

        if (currentDialog.currentSentence != null)
        {
            characterNameText.text = currentDialog.currentSentence.characterName;
            if (shouldShowCharactersSprite)
            {
                if (charactersSprites.ContainsKey(currentDialog.currentSentence.characterName))
                {
                    talkingCharacterImage.sprite = charactersSprites[currentDialog.currentSentence.characterName];
                    if (!talkingCharacterImage.gameObject.activeSelf)
                        talkingCharacterImage.gameObject.SetActive(true);
                }
                else
                    talkingCharacterImage.gameObject.SetActive(false);
            }
            else
                talkingCharacterImage.gameObject.SetActive(false);

            if (currentDialog.currentSentence.GetType() != typeof(QuestioningSentence))
            {
                StartCoroutine(Autotype(sentenceText, currentDialog.currentSentence.content, false));
                //Prepare next sentence
                currentDialog.SetNextSentence();

            }
            else
            {
                StartCoroutine(Autotype(sentenceText, currentDialog.currentSentence.content, true));

            }
        }
        else
        {
            //OnDialogOver?.Invoke();

            int currentDialogId = DialogsLoader.Instance.dialogsDico.FirstOrDefault(x => x.Value == currentDialog).Key;

            DialogEvents? dialogEvent = FindCorrespondingEvents(currentDialogId);
            if (dialogEvent != null)
            {
                dialogEvent.Value.EndEvents?.Invoke();
            }

        }
    }

    private void InstantiateAnswers(QuestioningSentence qSentence)
    {
        dialogBox.GetComponent<Image>().raycastTarget = false;
        foreach (Answer answer in qSentence.answers)
        {
            Debug.Log(answer.answerText + " - " + answer.sentence.content + " - ");
            if (answer.sentence.GetNextSentence() != null)
            {
                Debug.Log(answer.sentence.GetNextSentence().content);
            }
            GameObject answerToInstantiate = Instantiate(answerPrefab, answersContainer);
            answerToInstantiate.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;
            answerToInstantiate.GetComponent<Button>().onClick.AddListener(delegate { currentDialog.SetNextSentence(answer.sentence); DisplayNextSentence(); FadeClearAnswers(answersFadeDuration); dialogBox.GetComponent<Image>().raycastTarget = true; });
            answerToInstantiate.GetComponent<GraphicFader>().FadeIn(answersFadeDuration);
        }
        StartCoroutine(ResizeChilds(dialogBox.transform));
    }

    public void ToggleCharacterSpriteVisibility()
    {
        shouldShowCharactersSprite = !shouldShowCharactersSprite;
        talkingCharacterImage.gameObject.SetActive(shouldShowCharactersSprite);
    }

    private void Update()
    {
        if (ShouldDisplaySkip())
        {
            showSkipTimer += Time.deltaTime;
            if (showSkipTimer >= showSkipTimerLimit)
                ToggleSkip(true);
        }
        else
        {
            if (showSkipTimer != 0f)
                showSkipTimer = 0f;
            if (skipImage.gameObject.GetComponent<Animator>().GetBool("Active"))
                ToggleSkip(false);
        }
    }

    private bool ShouldDisplaySkip()
    {
        if (!typingSentence && currentDialog != null)
            return true;
        else
            return false;
    }

    private void ToggleSkip(bool active)
    {
        skipImage.gameObject.GetComponent<Animator>().SetBool("Active", active);
    }

    private IEnumerator Autotype(TextMeshProUGUI textMesh, string content, bool isQuestionningSentence)
    {
        textMesh.text = "";
        typingSentence = true;
        for (int i = 0; i < content.Length; i++)
        {
            if (typingSentence)
            {
                sentenceText.text += content[i];
                yield return new WaitForSeconds(Time.deltaTime / sentenceTypingSpeed);
            }
            else
            {
                sentenceText.text = content;
                break;
            }
        }

        if (typingSentence)
            typingSentence = false;
        if (isQuestionningSentence)
        {
            InstantiateAnswers((QuestioningSentence)currentDialog.currentSentence);
        }

        StopCoroutine(Autotype(textMesh, content, isQuestionningSentence));
    }

    private DialogEvents? FindCorrespondingEvents(int id)
    {
        foreach (var item in dialogsEvents)
        {
            if (id == item.dialogID)
            {
                return item;
            }
        }
        return null;
    }

    public void StopDialog()
    {
        currentDialog = null;
        //dialogBox.SetActive(false);
    }

    public void SetNewDialog(int dialogId)
    {
        currentDialog = DialogsLoader.Instance.dialogsDico[dialogId];
        Debug.Log(currentDialog);
        Debug.Log(dialogId);

    }

    public void SetNextDialog()
    {
        int currentDialogId = DialogsLoader.Instance.dialogsDico.FirstOrDefault(x => x.Value == currentDialog).Key;
        int nextDialogId = int.MaxValue;

        foreach (var item in DialogsLoader.Instance.dialogsDico)
        {
            if (item.Key < nextDialogId && item.Key > currentDialogId)
            {
                nextDialogId = item.Key;
            }
        }
        currentDialog = DialogsLoader.Instance.dialogsDico[nextDialogId];
    }

    public void StartDialogDelayed(float delay)
    {
        Invoke(nameof(StartDialog), delay);
    }

    public IEnumerator ResizeChilds(Transform parent)
    {
        yield return new WaitForEndOfFrame();

        foreach (RectTransform item in parent.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        }

        StopCoroutine(ResizeChilds(parent));
    }

    public void FadeClearAnswers(float disappaearDuration)
    {
        foreach (Transform child in answersContainer)
        {
            child.GetComponent<GraphicFader>().FadeOut(disappaearDuration);
            child.GetComponent<GraphicFader>().EndFadeOutText.AddListener(() => { Destroy(child.gameObject); });
        }
    }

}

[System.Serializable]
public struct DialogEvents
{
    public int dialogID;
    public UnityEvent StartEvents;
    public UnityEvent EndEvents;
}

[System.Serializable]
public struct Character
{
    public string name;
    public Sprite dialogSprite;
}
