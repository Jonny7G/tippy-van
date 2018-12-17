using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    
    public List<Poolable> activeObjects;
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField]private GameEvent OnPoolReset;
    private Dictionary<int, Queue<Poolable>> PooledObjects = new Dictionary<int, Queue<Poolable>>();
    private Dictionary<int, Poolable> ReferenceList = new Dictionary<int, Poolable>();

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
        instance = this;
    }
    public void SetPool(Poolable obj,int key,int amount)
    {
        PooledObjects.Add(key,new Queue<Poolable>());
        ReferenceList.Add(key, obj);

        for(int i = 0; i < amount; i++)
        {
            Poolable newOb = Instantiate(obj);
            newOb.Key = key;
            PooledObjects[key].Enqueue(newOb);
            newOb.gameObject.SetActive(false);
        }
    }
    public Poolable GetFromPool(int key)
    {
        try
        {
            Poolable ob = PooledObjects[key].Dequeue();
            activeObjects.Add(ob);
            ob.gameObject.SetActive(true);
            return ob;
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning(ex.Message+" added an additional object");

            Poolable newOb = Instantiate(ReferenceList[key]);
            newOb.Key = key;
            PooledObjects[key].Enqueue(newOb);
            
            Poolable ob = PooledObjects[key].Dequeue();
            activeObjects.Add(ob);
            return ob;
        }
    }
    public void EnterPool(int key,Poolable poolObject)
    {
        PooledObjects[key].Enqueue(poolObject);
        activeObjects.Remove(poolObject);
        poolObject.gameObject.SetActive(false);
    }
    public void ResetPool()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            activeObjects[i].EndReached();
        }
        OnPoolReset.Raise();
    }
    public int GetUniqueID()
    {
        int id = Random.Range(0, 1000);

        while (PooledObjects.ContainsKey(id))
        {
            id = Random.Range(0, 1000);
        }

        return id;
    }
}
