using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Header("점수 목록")]
    [SerializeField] private Transform flappyBirdList;
    [SerializeField] private Transform miniGame2List;

    [Header("프리팹")]
    [SerializeField] private GameObject scorePrefab;

    [Header("간격")]
    [SerializeField] private float startYPos;
    [SerializeField] private float spacing;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        RefreshLeaderboards();
    }

    private void Init()
    {
        startYPos = 0f;
        spacing = 22f;
    }

    public void RefreshLeaderboards()
    {
        DisplayLeaderboard("FlappyBird", flappyBirdList);
        DisplayLeaderboard("DodgePoop", miniGame2List);
    }

    private void DisplayLeaderboard(string gameName, Transform listParent)
    {
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        List<ScoreData> top10 = LeaderboardManager.Instance.GetTop10(gameName);

        if (top10.Count > 0)
        {
            for (int i = 0; i < top10.Count; i++)
            {
                GameObject entryObj = Instantiate(scorePrefab, listParent);
                Text entryText = entryObj.GetComponent<Text>();
                RectTransform rect = entryObj.GetComponent<RectTransform>();

                rect.anchoredPosition = new Vector2(0, startYPos - (i * spacing));

                entryText.text = $"{i + 1}. {top10[i].playerName} {top10[i].score}점";
            }
        }
        else
        {
            GameObject emptyObj = Instantiate(scorePrefab, listParent);
            RectTransform rectTransform = emptyObj.GetComponent<RectTransform>();
            Text emptyText = emptyObj.GetComponent<Text>();

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }

            if (emptyText != null)
            {
                emptyText.text = "기록 없음";
            }
        }
    }
}
