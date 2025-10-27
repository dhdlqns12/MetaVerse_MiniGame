using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ScoreData
{
    public string playerName;
    public int score;

    public ScoreData(string _name, int _score)
    {
        this.playerName = _name;
        this.score = _score;
    }
}

[Serializable]
public struct GameLeaderboard 
{
    public string gameName;
    public List<ScoreData> scores;

    public GameLeaderboard(string _gameName)
    {
        this.gameName = _gameName;
        this.scores = new List<ScoreData>();
    }
}

[Serializable]
public struct LeaderboardData
{
    public List<GameLeaderboard> gameLeaderboards;

    public LeaderboardData(bool init)
    {
        gameLeaderboards = new List<GameLeaderboard>();
    }
}
