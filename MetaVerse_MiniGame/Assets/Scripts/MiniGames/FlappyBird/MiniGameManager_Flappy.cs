using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager_Flappy : MonoBehaviour,IMiniGameManager
{
    public bool IsGameOver { get; private set; }

    [Header("Á¡¼ö")]
    public int curScroe_Flappy;

    private void OnEnable()
    {
        MiniGameManagerRegistry.Register_MiniGame(this);
    }

    private void OnDisable()
    {
        MiniGameManagerRegistry.Unregister_MIniGame(this);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        curScroe_Flappy = 0;
        IsGameOver = false;
    }

    #region FlappyBird
    public void GameOver()
    {
        SaveBsetScore();
        SaveLeaderboard();
        IsGameOver = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SaveBsetScore();
        SaveLeaderboard();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        if (IsGameOver) return;

        curScroe_Flappy += score;
        GameManager.Instance.Gold += score*100;
    }

    public void SaveBsetScore()
    {
        if (GameManager.Instance.gameData.bestScore_Flappy < curScroe_Flappy)
        {
            GameManager.Instance.gameData.bestScore_Flappy = curScroe_Flappy;
        }
    }

    private void SaveLeaderboard()
    {
        string playerName = GameManager.Instance.playerData.playerName;
        LeaderboardManager.Instance.InputScore("FlappyBird", playerName, curScroe_Flappy);
    }
    #endregion
}
