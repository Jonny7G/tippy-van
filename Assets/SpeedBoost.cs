using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private FloatVariable worldSpeed;
    [SerializeField] private float additiveSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("speed change triggered");
        worldSpeed.Value += additiveSpeed;
    }
}