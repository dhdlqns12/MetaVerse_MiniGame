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

                ScoreData newScore = new ScoreData(playerName, score); // 값 타입 복사기 때문에 GC가 발생안하고 함수 끝나면 메모리 할당 해제, List에 추가하려면 새객체 필요해서 사용. List에는 복사본의 값만 남음
                gameLeaderboard.scores.Add(newScore);

                gameLeaderboard.scores.Sort((a, b) => b.score.CompareTo(a.score)); // 내림차 순 정렬

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