using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ShopData))]
public class ShopDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Reset Shop"))
            {
                ShopData Shop = (ShopData)target;
                Shop.ResetData();
            }
        }
    }
}