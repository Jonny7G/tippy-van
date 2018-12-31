using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInputOnTrigger : MonoBehaviour
{
    [SerializeField] private BoolVariable canTurn;
    [SerializeField] private bool val;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canTurn.Value = val;
    }
}
