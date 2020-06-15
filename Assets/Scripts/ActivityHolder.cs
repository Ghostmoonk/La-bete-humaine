using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ActivityHolder : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI contentTextMesh;
    [SerializeField] protected TextMeshProUGUI paratextTextMesh;

    public virtual void SetContent(NoteActivity activityData)
    {
        contentTextMesh.text = activityData.fullText;
        paratextTextMesh.text = activityData.paratext;
    }

    public abstract void CompleteActivity();
}
