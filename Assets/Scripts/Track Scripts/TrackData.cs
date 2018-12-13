using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackData : MonoBehaviour,IEntity
{
    public Vector2 BackConnection { get { return _backConnection.position; } }
    public string PoolTag { get { return _poolTag; } }

    [SerializeField] private string _poolTag;
    [SerializeField] private Transform _backConnection;
    [SerializeField] private Sprite[] tileDeviations;

    private SpriteRenderer spriteRenderer;
    [SerializeField]private CoinBehaviour[] containedCoins;

    void Awake() //needs to be in Awake so its called before OnEnable.
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (CoinBehaviour coin in containedCoins)
            coin.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        ChangeDeviation();
    }
    void ChangeDeviation()
    {
        foreach (CoinBehaviour coin in containedCoins)
            coin.RollSpawnChance();

        Debug.Log(Random.Range(0, tileDeviations.Length-1));
        spriteRenderer.sprite = tileDeviations[Random.Range(0, tileDeviations.Length-1)];
    }
    public void EndReached()
    {
        ObjectPooler.instance.RequeObject(PoolTag, gameObject);
        TrackGenerator.instance.RemoveActive(this);
    }
}