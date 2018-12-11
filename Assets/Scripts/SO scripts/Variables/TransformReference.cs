using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransformReference", menuName = "Variables/Primatives/Reference/Transform Reference", order = 4)]
public class TransformReference : ScriptableObject
{
    public TransformVariable variable;
    public Transform value { get { return variable.Value; } }
}