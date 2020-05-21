using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net.Mime;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CustomEditor(typeof(ContentsSupport))]
public class ContentsSupportEditor : Editor
{
    SerializedProperty contentsList;
    ContentsSupport contentsSupport;
    SerializedObject targetObject;
    bool[] foldouts;
    int listSize;

    private void OnEnable()
    {
        contentsSupport = (ContentsSupport)target;
        targetObject = new SerializedObject(contentsSupport);
        contentsList = targetObject.FindProperty("contentsType");
        foldouts = new bool[contentsList.arraySize];
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        listSize = contentsList.arraySize;
        listSize = EditorGUILayout.IntField("Number of contents :", listSize);

        if (listSize != contentsList.arraySize)
        {
            foldouts = new bool[listSize];
            while (listSize > contentsList.arraySize)
            {
                contentsList.InsertArrayElementAtIndex(contentsList.arraySize);
            }
            while (listSize < contentsList.arraySize)
            {
                contentsList.DeleteArrayElementAtIndex(contentsList.arraySize - 1);
            }
        }

        for (int i = 0; i < contentsList.arraySize; i++)
        {
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Content - " + i, true);
            if (foldouts[i])
            {
                SerializedProperty listRef = contentsList.GetArrayElementAtIndex(i);
                SerializedProperty type = listRef.FindPropertyRelative("type");
                SerializedProperty id = listRef.FindPropertyRelative("id");
                SerializedProperty CompleteEvent = listRef.FindPropertyRelative("CompleteEvent");
                SerializedProperty OnSelectEvent = listRef.FindPropertyRelative("OnSelectEvent");


                EditorGUILayout.PropertyField(type, true);
                EditorGUILayout.PropertyField(id, true);
                EditorGUILayout.PropertyField(CompleteEvent, true);

                if (type.enumValueIndex == ContentType.OpenQuestion.GetHashCode())
                {
                    //EditorGUILayout.PropertyField(contentsType.GetArrayElementAtIndex(i).FindPropertyRelative("OnSelectEvent"), true);
                    EditorGUILayout.PropertyField(OnSelectEvent);
                }
            }
        }
        targetObject.ApplyModifiedProperties();
    }
}
