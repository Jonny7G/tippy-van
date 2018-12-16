using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffectBehaviour : MonoBehaviour
{
    private ParticleSystem effect;
    [SerializeField] private TransformReference recentCoin;

    private void Start()
    {
        effect = GetComponent<ParticleSystem>();
    }

    public void EmitEffect()
    {
        transform.position = recentCoin.value.position;
        effect.Emit(30);
    }
}