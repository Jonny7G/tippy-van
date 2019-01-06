using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleTypeVariable", menuName = "Variables/Custom/Variable/Vehicle Variable", order = 1)]
public class VehicleVariable : ScriptableObject
{
    public Vehicles value;
}
