using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData : MonoBehaviour
{
    [SerializeField] private IntVariable scoreVar;
    [SerializeField] private IntVariable highScoreVar;
    [Header("Events")]
    [SerializeField] private GameEvent OnScoreSet;
    private string savePath;
    private ScoreData scoreData;

    private void Start()
    {
        savePath = System.IO.Path.Combine(Application.persistentDataPath, "ProgressData.txt");

        ResetScore();
        InitializeScore();
    }

    public void ResetScore() => scoreVar.Value = 0;

    public void ResetHighScore()
    {
        if(Application.IsPlaying(gameObject))
        {
            highScoreVar.Value = scoreData.HighScore = 0;
            SavingSystem.SaveProgress(scoreData, savePath);
        }
    }

    public void InitializeScore()
    {
        scoreData = new ScoreData();
        SavingSystem.LoadProgress(out scoreData, savePath);
        highScoreVar.Value = scoreData.HighScore;
    }

    public void SetScore()
    {
        scoreData.TotalScore += scoreVar.Value;

        if (scoreVar.Value > scoreData.HighScore)
        {
            highScoreVar.Value = scoreData.HighScore = scoreVar.Value;
        }
        PlayGamesManager.UploadScore(GPGSIds.leaderboard_highscores, scoreVar.Value);
        SavingSystem.SaveProgress(scoreData, savePath);
        OnScoreSet.Raise();
    }
}

[System.Serializable]
public struct ScoreData
{
    public int HighScore;
    public int TotalScore;
}