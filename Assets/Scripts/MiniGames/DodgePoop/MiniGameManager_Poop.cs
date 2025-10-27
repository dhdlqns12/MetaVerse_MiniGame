using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager_Poop : MonoBehaviour, IMiniGameManager
{
    public bool IsGameOver { get; private set; }

    [Header("점수")]
    public int curScore_Poop;

    [Header("타이머")]
    public float survivalTime = 0f;

    [Header("스포너")]
    [SerializeField] private PoopSpawner poopSpawner;

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

    private void Update()
    {
        if (!IsGameOver)
        {
            survivalTime += Time.deltaTime;
            curScore_Poop = (int)survivalTime*2;
        }
    }

    private void Init()
    {
        curScore_Poop = 0;
        survivalTime = 0f;
        IsGameOver = false;
    }

    #region 똥피하기
    public void GameOver()
    {
        SaveBestScore();
        SaveLeaderboard();
        IsGameOver = true;

        if (poopSpawner != null)
        {
            poopSpawner.StopSpawning();
        }

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SaveBestScore();
        SaveLeaderboard();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
    }

    public void SaveBestScore()
    {
        if (GameManager.Instance.gameData.bestScore_Poop < curScore_Poop)
        {
            GameManager.Instance.gameData.bestScore_Poop = curScore_Poop;
        }
    }

    private void SaveLeaderboard()
    {
        string playerName = GameManager.Instance.playerData.playerName;
        LeaderboardManager.Instance.InputScore("DodgePoop", playerName, curScore_Poop);

        GameManager.Instance.Gold += curScore_Poop * 5;
    }
    #endregion
}