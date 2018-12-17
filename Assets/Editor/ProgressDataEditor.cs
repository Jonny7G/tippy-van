using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ProgressData))]
public class ProgressDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ProgressData data = (ProgressData)target;

        if(GUILayout.Button("Reset High-Score"))
        {
            data.ResetHighScore();
        }
    }
}
