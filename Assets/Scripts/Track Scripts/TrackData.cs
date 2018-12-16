using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Poolable))]
public class TrackData : MonoBehaviour
{
    public Vector2 BackConnection { get { return _backConnection.position; } }
    
    [SerializeField] private Transform _backConnection;
    [SerializeField] private bool hasDeviations;
    [SerializeField] private Sprite[] tileDeviations;
    [SerializeField] private CoinBehaviour[] containedCoins;

    private SpriteRenderer spriteRenderer;
    private Poolable myPoolable;

    void Awake() //needs to be in Awake so its called before OnEnable.
    {
        if (hasDeviations)
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

        if (hasDeviations)
            spriteRenderer.sprite = tileDeviations[Random.Range(0, tileDeviations.Length-1)];
    }
}