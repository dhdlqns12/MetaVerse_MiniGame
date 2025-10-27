using System.Collections.Generic;
using UnityEngine;
using System;

public class LeaderboardManager : Singleton<LeaderboardManager>
{
    public void InputScore(string gameName, string playerName, int score)
    {
        LeaderboardData data = GameManager.Instance.leaderboardData;

        bool found = false;

        for (int i = 0; i < data.gameLeaderboards.Count; i++)
        {
            if (data.gameLeaderboards[i].gameName == gameName)
            {
                GameLeaderboard gameLeaderboard = data.gameLeaderboards[i];

                ScoreData newScore = new ScoreData(playerName, score); // �� Ÿ�� ����� ������ GC�� �߻����ϰ� �Լ� ������ �޸� �Ҵ� ����, List�� �߰��Ϸ��� ����ü �ʿ��ؼ� ���. List���� ���纻�� ���� ����
                gameLeaderboard.scores.Add(newScore);

                gameLeaderboard.scores.Sort((a, b) => b.score.CompareTo(a.score)); // ������ �� ����

                if (gameLeaderboard.scores.Count > 10)
                {
                    gameLeaderboard.scores.RemoveRange(10, gameLeaderboard.scores.Count - 10);
                }

                data.gameLeaderboards[i] = gameLeaderboard;
                found = true;
                break;
            }
        }

        if (!found)
        {
            GameLeaderboard newGameLeaderboard = new GameLeaderboard(gameName);
            ScoreData newScore = new ScoreData(playerName, score);
            newGameLeaderboard.scores.Add(newScore);
            data.gameLeaderboards.Add(newGameLeaderboard);
        }

        GameManager.Instance.leaderboardData = data;
        SaveManager.Instance.SaveLeaderboardData(data);
    }

    public List<ScoreData> GetTop10(string gameName)
    {
        LeaderboardData data = GameManager.Instance.leaderboardData;

        for (int i = 0; i < data.gameLeaderboards.Count; i++)
        {
            if (data.gameLeaderboards[i].gameName == gameName)
            {
                return data.gameLeaderboards[i].scores;
            }
        }

        return new List<ScoreData>();
    }
}