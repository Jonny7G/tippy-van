using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject deathPanel;
    [Header("SO's")]
    [SerializeField] private GameStateManager gameState;

    private int score = 0;
    
    public void ActivateDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    public void DeactivateDeathPanel()
    {
        deathPanel.SetActive(false);
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.SetText(score.ToString());
    }

    public void AddScore()
    {
        score++;
        scoreText.SetText(score.ToString());
    }

    public void QuitGame() => Application.Quit();
}