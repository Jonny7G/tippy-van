using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackData : MonoBehaviour,IEntity
{
    public Vector2 BackConnection { get { return _backConnection.position; } }
    public Vector2 FrontConnection { get { return _frontConnection.position; } }
    public string PoolTag { get { return _poolTag; } }

    [SerializeField] private string _poolTag;
    [SerializeField] private Transform _backConnection;
    [SerializeField] private Transform _frontConnection;

    public void EndReached()
    {
        ObjectPooler.instance.RequeObject(PoolTag, gameObject);
        TrackGenerator.instance.RemoveActive(this);
    }
}