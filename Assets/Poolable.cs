using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Poolable : MonoBehaviour
{
    public int Key { get; set; }

    private ObjectPooling pooler;
    public event Action OnEndReached;

    public void EndReached()
    {
        pooler.EnterPool(Key,this);
        OnEndReached?.Invoke();
    }
}