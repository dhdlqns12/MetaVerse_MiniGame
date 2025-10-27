using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GameNPC : NPCBase
{
    public string gameSceneName;
    public PlayerController.StrategyType strategyType;
    public string NPCName { get; private set; }
    public string gameDescription;

    public void StartGame()
    {
        GameManager.Instance?.LoadScene(gameSceneName, strategyType);
    }
}
