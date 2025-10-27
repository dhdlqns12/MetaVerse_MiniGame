using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGameManager 
{
    bool IsGameOver { get; }
    void AddScore(int score);
    void GameOver();
    void RestartGame();
}
