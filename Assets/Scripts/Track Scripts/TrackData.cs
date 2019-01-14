using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackData : MonoBehaviour,IPoolable
{
    public Vector2 BackConnection { get { return _backConnection.position; } }

    [SerializeField] private Transform _backConnection;
    [SerializeField] public CoinBehaviour[] containedCoins;
 
    #region Poolable
    public int Key { get; set; }

    public GameObject PoolObject => _poolObject;

    [SerializeField] private GameObject _poolObject;

    public void EndReached()
    {
        ObjectPooling.instance.EnterPool(Key, this);
    }
    #endregion
    
}