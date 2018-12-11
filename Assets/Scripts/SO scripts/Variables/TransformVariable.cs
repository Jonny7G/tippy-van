using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransformReference", menuName = "Variables/Primatives/Variable/Transform Variable", order = 4)]
public class TransformVariable : ScriptableObject
{
    public Transform Value;
}