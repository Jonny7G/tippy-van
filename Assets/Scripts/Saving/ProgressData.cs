using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ProgressData : MonoBehaviour
{
    public static ProgressData instance;

    [SerializeField] private IntVariable scoreVar;
    [SerializeField] private IntVariable highScoreVar;
    [SerializeField] private IntVariable totalScoreVar;

    [Header("Events")]
    [SerializeField] private GameEvent OnScoreSet;

    private string savePath;
    private ScoreData scoreData;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        if (instance == this)
        {
            savePath = System.IO.Path.Combine(Application.persistentDataPath, "ProgressData.txt");

            ResetScore();
            InitializeScore();
        }
    }

    #region reload behaviour 
    private void OnEnable()
    {
        GameState.instance.OnGameOver += SetScore;
        GameState.instance.OnGameReload += ResetScore;
    }
    private void OnDisable()
    {
        GameState.instance.OnGameOver -= SetScore;
        GameState.instance.OnGameReload -= ResetScore;
    }
    #endregion

    public void ResetScore() => scoreVar.Value = 0;

    public void ResetHighScore()
    {
        if(Application.IsPlaying(gameObject))
        {
            highScoreVar.Value = scoreData.HighScore = 0;
            SavingSystem.SaveProgress(scoreData, savePath);
        }
    }
    public void AddScore()
    {
        totalScoreVar.Value += 1000;
        scoreData.TotalScore = totalScoreVar.Value;
        SavingSystem.SaveProgress(scoreData, savePath);
    }
    public void Unlocked(int cost)
    {
        scoreData.TotalScore -= cost;
        totalScoreVar.Value = scoreData.TotalScore;
        SavingSystem.SaveProgress(scoreData, savePath);
        OnScoreSet.Raise();
    }

    public void InitializeScore()
    {
        scoreData = new ScoreData(0,0);
        try
        {
            SavingSystem.LoadProgress(out scoreData, savePath);
        }
        catch (System.Exception)
        {
            SavingSystem.SaveProgress(scoreData, savePath);
        }

        highScoreVar.Value = scoreData.HighScore;
        totalScoreVar.Value = scoreData.TotalScore;

        OnScoreSet.Raise();
    }

    public void SetScore()
    {
        totalScoreVar.Value += scoreVar.Value;
        scoreData.TotalScore = totalScoreVar.Value;

        if (scoreVar.Value > scoreData.HighScore)
        {
            highScoreVar.Value = scoreData.HighScore = scoreVar.Value;
        }
        PlayGamesManager.UploadScore(GPGSIds.leaderboard_highscores, scoreVar.Value);
        //uploads everytime as the google play services handles whether or not its valid for us.(also handles setting daily,weekly and all time scores).

        SavingSystem.SaveProgress(scoreData, savePath);
        OnScoreSet.Raise();
    }
}

[System.Serializable]
public struct ScoreData
{
    public int HighScore;
    public int TotalScore;
    
    public ScoreData (int highScore,int totalScore)
    {
        HighScore = highScore;
        TotalScore = totalScore;
    }
}