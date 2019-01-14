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
    public event Action OnGameRestart;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            GameActive = false;
            Application.targetFrameRate = 60;
        }
    }
    public void GameEnd()
    {
        GameActive = false;
        OnGameOver?.Invoke();
    }
    public void GameStart()
    {
        Debug.Log("game start");
        GameActive = true;
        OnGameStart?.Invoke();
    }
    public void GameReload()
    {
        Debug.Log("game reload");
        GameActive = true;
        OnGameReload?.Invoke();
    }
    public void GameRestart()
    {
        Debug.Log("Game scene reloaded");
        OnGameRestart?.Invoke();
    }
}