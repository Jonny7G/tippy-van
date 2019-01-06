using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    int Key { get; set; }
    GameObject PoolObject { get; }
    void EndReached();
}
