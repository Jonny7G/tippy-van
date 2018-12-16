using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
public class GameStateManager : ScriptableObject
{
    public bool GameActive {
        get;
        private set;
    }

    //public event Action onGameReload;
    [SerializeField] private GameEvent OnGameReload;
    //public event Action onGameOver;
    [SerializeField] private GameEvent onGameOver;
    //public event Action playerFallen;
    [SerializeField] private GameEvent onPlayerFallen;

    public void RestartGame()
    {
        GameActive = true;
        OnGameReload.Raise();
    }

    public void PlayerFallen()
    {
        onPlayerFallen.Raise();
    }

    public void GameOver()
    {
        onGameOver.Raise();
        GameActive = false;
    }

    private void OnEnable()
    {
        GameActive = true;
    }
}