using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject deathPanel;
    private int score = 0;

    private void Start()
    {
        ResetScore();
    }

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
        scoreText.text = score.ToString();
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void QuitGame() => Application.Quit();
}