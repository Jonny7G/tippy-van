using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesManager : MonoBehaviour
{
    public static PlayGamesManager instance;
    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();
    }

    private void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    #region Leaderboards 
    public static void UploadScore(string id,long score)
    {
        Social.ReportScore(score, id, success=> { });
    }
    public static void ShowLeaderBoardUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboards
}