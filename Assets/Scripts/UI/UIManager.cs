using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private UnityEvent OnGameReload;
    [SerializeField] private UnityEvent OnGameOver;
    [SerializeField] private UnityEvent OnGameStart;
    private int score = 0;
    private bool wasLoaded;

    #region reload behaviour
    private void OnEnable()
    {
        GameState.instance.OnGameStart += InvokeStart;
        GameState.instance.OnGameReload += InvokeReload;
        GameState.instance.OnGameOver += InvokeOnGameOver;
    }
    private void OnDisable()
    {
        GameState.instance.OnGameStart -= InvokeStart;
        GameState.instance.OnGameReload -= InvokeReload;
        GameState.instance.OnGameOver -= InvokeOnGameOver;
    }
    private void InvokeStart() => OnGameStart?.Invoke();
    private void InvokeReload() => OnGameReload?.Invoke();
    private void InvokeOnGameOver() => OnGameOver?.Invoke();
    #endregion reload behaviour
    private void Start()
    {
        ResetScore();

        wasLoaded = false;
        soundToggle.isOn = AudioListener.volume == 0;
        wasLoaded = true;
    }
    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
    public void ReloadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
    public void OnStartGame() => GameState.instance.GameStart();
    public void OnRestartGame() => GameState.instance.GameReload();
    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void ShowLeaderBoardUI()
    {
        PlayGamesManager.ShowLeaderBoardUI();
    }
    public void PlayButtonPressedSound()
    {
        AudioManager.instance.PlaySound("UI sfx");
    }
    public void MuteToggleAction()
    {
        if (wasLoaded)
        {
            AudioListener.volume = 1 - AudioListener.volume;
        }
    }
    public void QuitGame() => Application.Quit();
}