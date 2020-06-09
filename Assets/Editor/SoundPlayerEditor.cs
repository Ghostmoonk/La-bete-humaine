using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor
{
    SerializedProperty loopDifferentSounds;
    SerializedProperty delayBetweenSounds;
    SoundPlayer soundPlayer;

    private void OnEnable()
    {
        soundPlayer = (SoundPlayer)target;

        loopDifferentSounds = serializedObject.FindProperty("loopDifferentSounds");
        delayBetweenSounds = serializedObject.FindProperty("delayBetweenSounds");
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (loopDifferentSounds.boolValue)
        {
            soundPlayer.delayBetweenSounds = EditorGUILayout.Slider("Delay Between Sounds", soundPlayer.delayBetweenSounds, 0f, 2f);
        }
    }
}
