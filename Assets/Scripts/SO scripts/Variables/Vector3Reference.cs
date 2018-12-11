using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVector3Reference", menuName = "Variables/Primatives/Reference/Vector3 Reference", order = 3)]
public class Vector3Reference : ScriptableObject
{
    public Vector3Variable variable;
    public Vector3 value { get { return variable.Value; } }
}