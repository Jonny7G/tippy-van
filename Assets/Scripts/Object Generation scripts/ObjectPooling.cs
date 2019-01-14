using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    
    public List<IPoolable> activeObjects;
    [SerializeField]private GameEvent OnPoolReset;
    private Dictionary<int, Queue<IPoolable>> PooledObjects = new Dictionary<int, Queue<IPoolable>>();
    private Dictionary<int, IPoolable> ReferenceList = new Dictionary<int, IPoolable>();

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        instance = this;

        activeObjects = new List<IPoolable>();
        
    }
    private void Start()
    {
        GameState.instance.OnGameReload += ResetPool;
        GameState.instance.OnGameStart += ResetPool;
    }
    #region reload behaviour 
    private void OnDisable()
    {
        GameState.instance.OnGameReload -= ResetPool;
        GameState.instance.OnGameStart -= ResetPool;
    }
    #endregion
    public void SetPool(IPoolable obj,int key,int amount)
    {

        PooledObjects.Add(key,new Queue<IPoolable>());
        ReferenceList.Add(key, obj);

        for(int i = 0; i < amount; i++)
        {
            GameObject Ob = Instantiate(obj.PoolObject);
            IPoolable poolable = Ob.GetComponent<IPoolable>();
            poolable.Key = key;
            PooledObjects[key].Enqueue(poolable);
            poolable.PoolObject.SetActive(false);
        }
    }
    public IPoolable GetFromPool(int key)
    {
        try
        {
            IPoolable poolable = PooledObjects[key].Dequeue();
            activeObjects.Add(poolable);
            poolable.PoolObject.SetActive(true);
            return poolable;
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning(ex.Message+" added an additional object");
            Debug.Log(ReferenceList[key].PoolObject.name);

            GameObject Ob = Instantiate(ReferenceList[key].PoolObject);
            IPoolable poolable = Ob.GetComponent<IPoolable>();
            poolable.Key = key;
            poolable.PoolObject.SetActive(true);
            
            activeObjects.Add(poolable);
            return poolable;
        }
    }
    public void EnterPool(int key,IPoolable poolObject)
    {
        Debug.Log("Entering pool");
        PooledObjects[key].Enqueue(poolObject);
        activeObjects.Remove(poolObject);
        poolObject.PoolObject.SetActive(false);
    }
    public void ResetPool()
    {
        Debug.Log("Pool resetting");
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            activeObjects[i].EndReached();
            Debug.Log("resetting pool object");
        }
        OnPoolReset.Raise();
    }
    private void Update()
    {
        Debug.Log(activeObjects.Count);
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