using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoolReference", menuName = "Variables/Primatives/Reference/Bool Reference", order = 2)]
public class BoolReference : ScriptableObject
{
    public BoolVariable variable;
    public bool value { get { return variable.Value; } }

    public void SetValue(bool val)
    {
        variable.Value = val;
    }
}