using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleFillGaps", menuName = "NoteActivity/SimpleFillGaps")]
public class FillGapsNoteActivity : NoteActivity
{
    [TextArea(7, 12)]
    public string gapsText;

}