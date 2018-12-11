using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoolVariable", menuName = "Variables/Primatives/Variable/Bool Variable", order = 2)]
public class BoolVariable : ScriptableObject
{
    public bool Value;
}