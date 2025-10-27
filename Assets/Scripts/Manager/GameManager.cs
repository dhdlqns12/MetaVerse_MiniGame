using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Singleton<GameManager>
{

    [Header("현재 전략")]
    public PlayerController.StrategyType curStrategy;

    public event Action<int> OnGoldChanged;

    public int Gold
    {
        get { return playerData.gold; }
        set
        {
            playerData.gold = value;
            OnGoldChanged?.Invoke(value);
        }
    }

    [Header("저장 데이터")]
    public PlayerData playerData;
    public GameData gameData;
    public LeaderboardData leaderboardData;

    protected override void Init()
    {
        curStrategy = PlayerController.StrategyType.MainGame;

        playerData = new PlayerData();
        gameData = new GameData();
        leaderboardData = new LeaderboardData(true);

        LoadGame();
    }

    public void LoadScene(string sceneName, PlayerController.StrategyType strategy)
    {
        curStrategy = strategy;
        SceneManager.LoadScene(sceneName);
    }

    public void ReturnMainGame()
    {
        LoadScene("Main", PlayerController.StrategyType.MainGame);
    }

    private void LoadGame()
    {
        if (SaveManager.Instance != null)
        {
            playerData = SaveManager.Instance.LoadPlayerData();
            gameData = SaveManager.Instance.LoadGameData();
            leaderboardData = SaveManager.Instance.LoadLeaderboardData();
        }
    }
}
