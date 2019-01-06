using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "VehicleTypeReference", menuName = "Variables/Custom/Reference/Vehicle type reference", order = 1)]
public class VehicleReference : ScriptableObject
{
    public VehicleVariable variable;
    public Vehicles Value { get { return variable.value; } }
}