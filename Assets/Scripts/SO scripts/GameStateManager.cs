using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameEvent OnGameReload;
    [SerializeField] private GameEvent onGameOver;
    [SerializeField] private GameEvent onPlayerFallen;

    [SerializeField] private BoolVariable GameActive;

    public void RestartGame()
    {
        OnGameReload.Raise();
    }

    public void PlayerFallen()
    {
        onPlayerFallen.Raise();
    }

    //public void GameOver()
    //{
    //    onGameOver.Raise();
    //    GameActive = false;
    //}
}