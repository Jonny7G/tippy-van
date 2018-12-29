using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
#pragma warning disable IDE0044 // Add readonly modifier
    [Header("Fields")]
    [Range(0,1)]
    [SerializeField] private float spawnChance;
    [Header("Variables")]
    [SerializeField] private TransformVariable recentCoin;
    [SerializeField] private IntVariable score;
    [Header("Events")]
    [SerializeField] private GameEvent coinCollected;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        score.Value++;
        AudioManager.instance.PlaySound("coin hit");
        animator.SetTrigger("pickedUp");
    }
    public void Deactivate() => gameObject.SetActive(false);
}