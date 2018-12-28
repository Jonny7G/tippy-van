using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackData : MonoBehaviour
{
    public Vector2 BackConnection { get { return _backConnection.position; } }
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private Transform _backConnection;
    [SerializeField] public CoinBehaviour[] containedCoins;
}