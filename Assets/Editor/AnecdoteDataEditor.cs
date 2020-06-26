using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(AnecdoteData))]
//public class AnecdoteDataEditor : Editor
//{
//    SerializedProperty anecdotesContent;
//    AnecdoteData anecdoteData;

//    private void OnEnable()
//    {
//        anecdoteData = (AnecdoteData)target;

//        anecdotesContent = serializedObject.FindProperty("anecdotesContent");
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        for (int i = 0; i < anecdotesContent.arraySize; i++)
//        {
//            switch (anecdotesContent.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex)
//            {
//                case 0:
//                    SerializedProperty text = anecdotesContent.GetArrayElementAtIndex(i).FindPropertyRelative("text");
//                    anecdoteData.anecdotesContent[i].text = EditorGUILayout.TextArea(text.stringValue);
//                    break;
//                case 1:
//                    SerializedProperty sprite = anecdotesContent.GetArrayElementAtIndex(i).FindPropertyRelative("sprite");
//                    EditorGUILayout.PropertyField(sprite);
//                    break;
//                default:
//                    break;
//            }
//        }
//    }
//}
