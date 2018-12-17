using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData : MonoBehaviour
{
    private ScoreData scoreData;
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private IntVariable score;
    private string savePath;

    private void Start()
    {
        savePath = System.IO.Path.Combine(Application.persistentDataPath, "ProgressData.txt");
        ResetScore();
        scoreData = new ScoreData();
        InitializeScore();
    }
    public void ResetScore() => score.Value = 0;
    public void ResetHighScore()
    {
        if(Application.IsPlaying(gameObject))
        {
            scoreData.HighScore = 0;
            SavingSystem.SaveProgress(scoreData, savePath);
        }
    }
    public void SetScore()
    {
        scoreData.TotalScore += score.Value;

        if (score.Value > scoreData.HighScore)
            scoreData.HighScore = score.Value;

        SavingSystem.SaveProgress(scoreData, savePath);
    }
    public void InitializeScore()
    {
        scoreData = SavingSystem.LoadProgress(scoreData, savePath);
    }
}

[System.Serializable]
public struct ScoreData
{
    public int HighScore;
    public int TotalScore;
}
