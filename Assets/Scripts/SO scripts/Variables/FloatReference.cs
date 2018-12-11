using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewFloatReference", menuName = "Variables/Primatives/Reference/Float Reference", order = 1)]
public class FloatReference : ScriptableObject
{
    public FloatVariable variable;
    public float value { get { return variable.Value; } }
}