using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxLagDistance;
    [SerializeField] private FloatReference worldSpeed;
    [SerializeField] private Vector3Reference worldDirection;

    private bool positionReached=true;

    public void ResetVariables()
    {
        positionReached = true;
    }
    public void StartMovement()
    {
        positionReached = false;
    }
    public void IncrementMovement()
    {
        if (!positionReached)
        {
            if (Vector2.Distance(transform.position, Vector3.zero) < maxLagDistance) //important that this is Vector2 version of distance
                transform.position += worldDirection.value/2 * worldSpeed.value * Time.deltaTime;
            else
                positionReached = true;
        }
    }
    public void Update()
    {
        if (positionReached)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, -100), 1.1f*Time.deltaTime);
    }
}