using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Variables/Primatives/Variable/Float Variable", order = 1)]
public class FloatVariable : ScriptableObject
{
    public float Value;

    public void SetFloat(float value)
    {
        Value = value;
    }
}