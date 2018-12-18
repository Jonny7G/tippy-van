using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActivation : MonoBehaviour
{
    [SerializeField]private BoolVariable GameActive;

    private void Awake()
    {
        GameActive.Value = false;
    }
    public void SetActive() => GameActive.Value = true;
}