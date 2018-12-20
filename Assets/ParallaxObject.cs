using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    public Vector3 LeftConnection { get => _leftConnection.position; }
    [SerializeField] private Transform _leftConnection;

    public Vector3 RightConnection { get => _rightConnection.position; }
    [SerializeField] private Transform _rightConnection;

    public Vector3 BottomConnection { get => _bottomConnection.position; }
    [SerializeField] private Transform _bottomConnection;

    public Vector3 TopConnection { get => transform.position; }
}