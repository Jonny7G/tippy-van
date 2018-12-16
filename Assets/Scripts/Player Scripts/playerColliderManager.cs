using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerColliderManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private Collider2D left;
    [SerializeField] private Collider2D right;

    [Header("References")]
    [SerializeField] private WorldDirectionReference worldDirection;
    

    public void SwitchCollider()
    {
        switch (worldDirection.direction)
        {
            case WorldDirection.left:
                left.enabled = true;
                right.enabled = false;
                break;
            case WorldDirection.right:
                right.enabled = true;
                left.enabled = false;
                break;
            default:
                break;
        }
    }
}