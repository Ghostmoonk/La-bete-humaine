using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI sentenceText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image skipImage;
    [SerializeField] Image talkingCharacterImage;

    Dialog currentDialog;

    public delegate void VoidDelegate();
    public VoidDelegate OnDialogOver;

    [Header("Characters")]
    [SerializeField] Character[] characters;
    Dictionary<string, Sprite> charactersSprites = new Dictionary<string, Sprite>();

    [Header("Events")]
    public DialogEvents[] dialogsEvents;

    [Range(0.1f, 20f)]
    [SerializeField] float sentenceTypingSpeed;

    bool typingSentence = false;
    [SerializeField] float showSkipTimerLimit;
    float showSkipTimer;

    bool shouldShowCharactersSprite = true;

    private void Start()
    {

        for (int i = 0; i < characters.Length; i++)
        {
            charactersSprites.Add(characters[i].name, characters[i].dialogSprite);
        }
    }
    public void StartDialog()
    {
        currentDialog.currentSentence.ShowSentences();
        dialogBox.SetActive(true);

        int currentDialogId = DialogsLoader.Instance.dialogsDico.FirstOrDefault(x => x.Value == currentDialog).Key;

        DialogEvents? dialogEvent = FindCorrespondingEvents(currentDialogId);
        if (dialogEvent != null)
        {
            dialogEvent.Value.StartEvents?.Invoke();
        }

        DisplayNextSentence();
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

            StartCoroutine(Autotype(sentenceText, currentDialog.currentSentence.content));

            //Prepare next sentence
            currentDialog.SetNextSentence();
        }
        else
        {
            OnDialogOver?.Invoke();
            int currentDialogId = DialogsLoader.Instance.dialogsDico.FirstOrDefault(x => x.Value == currentDialog).Key;

            DialogEvents? dialogEvent = FindCorrespondingEvents(currentDialogId);
            if (dialogEvent != null)
            {
                dialogEvent.Value.EndEvents?.Invoke();
            }

        }
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

    private IEnumerator Autotype(TextMeshProUGUI textMesh, string content)
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

        StopCoroutine(Autotype(textMesh, content));
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
        dialogBox.SetActive(false);
    }

    public void SetNewDialog(int dialogId)
    {
        currentDialog = DialogsLoader.Instance.dialogsDico[dialogId];
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
