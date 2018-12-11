using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWorldDirectionReference", menuName = "Variables/Custom/Reference/WorldDirection Reference", order = 2)]

public class WorldDirectionReference : ScriptableObject
{
    public WorldDirectionVariable variable;

    public WorldDirection direction { get { return variable.value; } }
}
