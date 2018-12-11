using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnTileVariable", menuName = "Variables/Custom/Variable/TurnTile Variable", order = 1)]
public class TurnTileVariable : ScriptableObject
{
    public TurnTile Value;
}