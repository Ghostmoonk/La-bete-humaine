using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TextHolder))]
public class TextHolderEditor : Editor
{
    //SerializedProperty showTitle;
    //SerializedProperty titleTextMesh;
    //TextHolder textHolder;

    //private void OnEnable()
    //{
    //    showTitle = serializedObject.FindProperty("showTitle");
    //    titleTextMesh = serializedObject.FindProperty("titleTextMesh");
    //    textHolder = (TextHolder)target;
    //}
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();


    //    if (textHolder.showTitle)
    //        textHolder.titleTextMesh = (TextMeshProUGUI)EditorGUILayout.ObjectField(textHolder.titleTextMesh, typeof(TextMeshProUGUI), true);
    //}
}
