using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour,IEntity
{
    [Header("Fields")]
    [Range(0,1)]
    [SerializeField] private float spawnChance;
    [SerializeField] private string _poolTag;
    [Header("Variables")]
    [SerializeField] private TransformVariable recentCoin;
    [Header("Events")]
    [SerializeField] private GameEvent coinCollected;
    
    public string PoolTag => _poolTag;

    public void EndReached()
    {
        ObjectPooler.instance.RequeObject(_poolTag, gameObject);
    }

    public void RollSpawnChance()
    {
        float roll = Random.Range(0f, 1f);
        if (roll <= spawnChance)
            gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        recentCoin.Value = transform;
        coinCollected.Raise();
        gameObject.SetActive(false);
    }
}