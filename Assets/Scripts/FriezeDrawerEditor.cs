using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FriezeDrawer))]
public class FriezeDrawerEditor : Editor
{
    FriezeDrawer friezeDrawer;

    private void OnEnable()
    {
        friezeDrawer = (FriezeDrawer)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("CreateFrieze", EditorStyles.miniButton))
        {
            friezeDrawer.DrawFrieze();
        }
        DrawDefaultInspector();

        if (GUI.changed)
        {
            Debug.Log("set dirty");
            EditorUtility.SetDirty(friezeDrawer);
        }
    }

}
