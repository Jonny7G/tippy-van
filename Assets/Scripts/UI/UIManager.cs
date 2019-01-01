using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Toggle soundToggle;
    private int score = 0;
    private bool wasLoaded;
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