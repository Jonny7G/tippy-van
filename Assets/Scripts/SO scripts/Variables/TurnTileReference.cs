using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnTileReference", menuName = "Variables/Custom/Reference/TurnTile Reference", order = 1)]
public class TurnTileReference : ScriptableObject
{
    public TurnTileVariable variable;
    public TurnTile value { get { return variable.Value; } }
}