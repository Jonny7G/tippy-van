using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    
    public static Vector3 GetIsometricCordinate(this Vector3 cord)
    {
        Vector3 newCord = new Vector3();
        newCord.x = cord.x - cord.y;
        newCord.y = (cord.x + cord.y) / 2;
        return newCord;
    }
}