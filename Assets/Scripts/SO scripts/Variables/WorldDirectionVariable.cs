using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWorldDirectionVariable", menuName = "Variables/Custom/Variable/WorldDirection Variable", order = 2)]
public class WorldDirectionVariable : ScriptableObject
{
    public WorldDirection value;
}
public enum WorldDirection { left,right}
