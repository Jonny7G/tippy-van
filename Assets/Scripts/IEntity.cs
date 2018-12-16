using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void EndReached();
    string PoolTag { get;}
    int Key { get; set; }
}