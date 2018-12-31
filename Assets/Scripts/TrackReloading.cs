using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TrackData))]
public class TrackReloading : MonoBehaviour
{
    [SerializeField] private bool hasDeviations;
    [SerializeField] private Sprite[] tileDeviations;

    private SpriteRenderer spriteRenderer;
    private TrackData trackData;

    void Awake() //needs to be in Awake so its called before OnEnable.
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trackData = GetComponent<TrackData>();

        if (trackData.containedCoins.Length > 0)
            foreach (CoinBehaviour coin in trackData.containedCoins)
                coin.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if(hasDeviations)
            ChangeDeviation();

        if (trackData.containedCoins.Length > 0)
            ReloadCoins();
    }

    void ReloadCoins()
    {
        foreach (CoinBehaviour coin in trackData.containedCoins)
            coin.RollSpawnChance();
    }

    void ChangeDeviation()
    {
        if (hasDeviations)
            spriteRenderer.sprite = tileDeviations[Random.Range(0, tileDeviations.Length - 1)];
    }
}