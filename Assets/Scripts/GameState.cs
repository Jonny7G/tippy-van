using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    public static bool GameActive { get; private set;}
    public static GameState instance;

    public event Action OnGameOver;
    public event Action OnGameStart;
    public event Action OnGameReload;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);

        GameActive = false;
        Application.targetFrameRate = 60;
    }
    public void GameEnd()
    {
        GameActive = false;
        OnGameOver?.Invoke();
    }
    public void GameStart()
    {
        OnGameStart?.Invoke();
        GameActive = true;
    }
    public void GameReload()
    {
        OnGameReload?.Invoke();
        GameActive = true;
    }
}