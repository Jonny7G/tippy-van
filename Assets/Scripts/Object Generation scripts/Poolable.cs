using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Poolable : MonoBehaviour
{
    public int Key { get; set; }

    private ObjectPooling pooler;
    public event Action OnEndReached;

    private void Awake()
    {
        pooler = ObjectPooling.instance;
    }

    public void EndReached()
    {
        Debug.Log(pooler);
        pooler.EnterPool(Key,this);
        OnEndReached?.Invoke();
    }
}