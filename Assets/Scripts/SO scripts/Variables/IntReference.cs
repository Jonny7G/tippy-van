using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new int ref", menuName = "Variables/Primatives/Reference/Int Reference", order = 5)]
public class IntReference : ScriptableObject
{
    public IntVariable variable;
    public int value { get { return variable.Value; } }
}
