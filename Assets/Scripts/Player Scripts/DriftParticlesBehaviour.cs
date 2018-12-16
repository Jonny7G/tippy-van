using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftParticlesBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3Reference playerDirection;

    private void Update()
    {
        transform.up = Vector3.MoveTowards(transform.up, playerDirection.value, Time.deltaTime * 10);
    }
}