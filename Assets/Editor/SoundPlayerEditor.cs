using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor
{
    SerializedProperty loopDifferentSounds;
    SerializedProperty delayBetweenSounds;
    SerializedProperty delayBetweenSoundsVariance;
    SoundPlayer soundPlayer;

    private void OnEnable()
    {
        soundPlayer = (SoundPlayer)target;

        loopDifferentSounds = serializedObject.FindProperty("loopDifferentSounds");
        delayBetweenSounds = serializedObject.FindProperty("delayBetweenSounds");
        delayBetweenSoundsVariance = serializedObject.FindProperty("delayBetweenSoundsVariance");
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (loopDifferentSounds.boolValue)
        {
            soundPlayer.delayBetweenSounds = EditorGUILayout.Slider("Delay Between Sounds", soundPlayer.delayBetweenSounds, 0f, 160f);
            soundPlayer.delayBetweenSoundsVariance = EditorGUILayout.Slider("Delay Variance", soundPlayer.delayBetweenSoundsVariance, 0f, 160f);
        }
    }
}
