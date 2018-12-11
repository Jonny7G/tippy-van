using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler instance;
    public List<GameObject> ActiveEntities;

    public Transform WorldParent { get { return _worldParent; } }

    [SerializeField] private List<Pool> pools;
    [SerializeField] private Transform _worldParent;

    [Header("SO's")]
    [SerializeField] private GameStateManager gameState;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    public void Awake()
    {
        instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = _worldParent;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetFromPool(string tag)
    {
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        ActiveEntities.Add(obj);
        return obj;
    }

    public void RequeObject(string tag,GameObject obj)
    {
        poolDictionary[tag].Enqueue(obj);
        ActiveEntities.Remove(obj);
        obj.SetActive(false);
    }

    public void ResetPool()
    {
        for (int i =ActiveEntities.Count-1;i>=0;i--)
            RequeObject(ActiveEntities[i].GetComponent<IEntity>().PoolTag, ActiveEntities[i]);
    }
}