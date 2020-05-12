using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TextHolder))]
public class TextHolderEditor : Editor
{
    SerializedProperty showTitle;
    SerializedProperty titleTextMesh;

    private void OnEnable()
    {
        showTitle = serializedObject.FindProperty("showTitle");
        titleTextMesh = serializedObject.FindProperty("titleTextMesh");
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TextHolder textHolder = (TextHolder)target;

        if (textHolder.showTitle)
            textHolder.titleTextMesh = (TextMeshProUGUI)EditorGUILayout.ObjectField(textHolder.titleTextMesh, typeof(TextMeshProUGUI), true);
    }
}
