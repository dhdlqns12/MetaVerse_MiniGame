using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Dodge : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button goToMainBtn;

    [Header("텍스트")]
    [SerializeField] private Text time_Txt;
    [SerializeField] private Text score_Txt;
    [SerializeField] private Text bestScore_Txt;

    [Header("패널")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("매니저")]
    [SerializeField] private MiniGameManager_Poop gameManager_Mini;

    private int lastScore = -1;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Update()
    {
        if (lastScore != gameManager_Mini.curScore_Poop)
        {
            UpdateScoreUI();
            lastScore = gameManager_Mini.curScore_Poop;
        }

        if (gameManager_Mini.IsGameOver && gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            ShowGameOver();
        }
    }

    #region 이벤트 구독/해제
    private void SubscribeEvents()
    {
        restartBtn?.onClick.AddListener(OnRestartClick);
        goToMainBtn?.onClick.AddListener(OnMainMenuClick);
    }

    private void UnsubscribeEvents()
    {
        restartBtn?.onClick.RemoveListener(OnRestartClick);
        goToMainBtn?.onClick.RemoveListener(OnMainMenuClick);
    }
    #endregion

    #region 버튼 이벤트
    private void OnRestartClick()
    {
        Debug.Log("재시작 버튼 클릭됨!");
        Time.timeScale = 1f;
        gameManager_Mini?.RestartGame();
    }

    private void OnMainMenuClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.ReturnMainGame();
    }
    #endregion

    #region UI 업데이트
    private void UpdateScoreUI()
    {
        if (gameManager_Mini == null) return;

        if (time_Txt != null)
        {
            time_Txt.text = $"Time: {gameManager_Mini.survivalTime:F0}";
        }

        if (score_Txt != null)
        {
            score_Txt.text = $"Score: {gameManager_Mini.curScore_Poop}";
        }

        if (bestScore_Txt != null && GameManager.Instance != null)
        {
            bestScore_Txt.text = $"Best: {GameManager.Instance.gameData.bestScore_Poop}";
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        UpdateScoreUI();
    }
    #endregion
}
