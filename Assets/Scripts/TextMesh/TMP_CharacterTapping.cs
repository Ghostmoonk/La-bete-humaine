using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//Handles the tapping on TextMesh
public class TMP_CharacterTapping : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textMeshs;
    AudioSource tappingSource;
    string[] texts;
    [SerializeField] float charactersTypingWait;

    public UnityEvent EndTypingEvent;

    private void Start()
    {
        tappingSource = GetComponent<AudioSource>();

        texts = new string[textMeshs.Length];

        for (int i = 0; i < textMeshs.Length; i++)
        {
            texts[i] = textMeshs[i].text;
            textMeshs[i].text = "";
        }
    }

    public void StartTyping()
    {
        StartCoroutine(PrintCharacter());
    }

    IEnumerator PrintCharacter()
    {
        for (int i = 0; i < textMeshs.Length; i++)
        {
            if (i != 0)
            {
                SoundManager.Instance.PlaySound(tappingSource, "machine-reset");
                yield return new WaitForSeconds(tappingSource.clip.length);
            }
            for (int j = 0; j < texts[i].Length; j++)
            {
                textMeshs[i].text += texts[i][j];
                if (texts[i][j] == '\n')
                {
                    SoundManager.Instance.PlaySound(tappingSource, "machine-reset");
                    yield return new WaitForSeconds(tappingSource.clip.length);
                }
                else
                {
                    SoundManager.Instance.PlaySound(tappingSource, "machine-typing");
                    yield return new WaitForSeconds(charactersTypingWait * Time.deltaTime);

                }

            }

        }
        EndTypingEvent?.Invoke();
        StopCoroutine(PrintCharacter());
    }
}
